using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.Abstract;
using DataLayer.DataContexts;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer.OrderServices.Concrete
{
    public class PlaceOrderService : ServiceErrors, IPlaceOrderService
    {
        private readonly AppIdentityDbContext efDbContext;

        public PlaceOrderService(AppIdentityDbContext efDbContext)
        {
            this.efDbContext = efDbContext;
        }

        public int PlaceOrder(PlaceOrderDto placeOrderDataIn)
        {
            var chosenIds = (placeOrderDataIn.Lines ?? Enumerable.Empty<PlaceOrderLineItemDto>())
                .Select(l => l.BookId)
                .ToArray();
            if (!chosenIds.Any())
            {
                AddError(errorMessage: "No items in your order.");
                return 0;
            }
            Dictionary<Guid, Book> booksDict = efDbContext.Books
                .Where(b => chosenIds.Contains(b.BookId))
                .ToDictionary(b => b.BookId);
            var orderLines = FormOrderLineItems(
                placeOrderLineItems: placeOrderDataIn.Lines,
                booksBaselineDict: booksDict);
            if (HasErrors)
            {
                return 0;
            }
            Order order = new Order
            {
                DateOrdered = DateTime.Now,
                Firstname = placeOrderDataIn.Firstname,
                LastName = placeOrderDataIn.Lastname,
                PhoneNumber = placeOrderDataIn.PhoneNumber,
                Lines = orderLines
            };
            efDbContext.Orders.Add(order);
            efDbContext.SaveChanges();
            return order.OrderId;
        }

        private IList<OrderLineItem> FormOrderLineItems(
            IEnumerable<PlaceOrderLineItemDto> placeOrderLineItems,
            Dictionary<Guid, Book> booksBaselineDict)
        {
            List<OrderLineItem> orderLines = new List<OrderLineItem>();
            foreach (var line in placeOrderLineItems)
            {
                if (line.Quantity <= 0 
                    || line.Quantity > GlobalConstants.MaxQuantityToBuy)
                {
                    AddError(errorMessage: "order line: invalid quantity value. ");
                    return orderLines;
                }
                if (!booksBaselineDict.ContainsKey(line.BookId))
                {
                    throw new InvalidOperationException(
                        message: $"A placing of order failed: book id={line.BookId} was missing");
                }
                var baselineBook = booksBaselineDict[line.BookId];
                orderLines.Add(new OrderLineItem
                {
                    Book = baselineBook,
                    BookId = baselineBook.BookId,
                    Quantity = line.Quantity
                });
            }
            return orderLines;
        }


    }
}
