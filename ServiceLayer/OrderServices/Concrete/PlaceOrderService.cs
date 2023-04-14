using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ServiceLayer.Abstract;
using Domain.Entities;
using ServiceDbAccessLayer.Orders;
using Domain;

namespace ServiceLayer.OrderServices.Concrete
{
    public class PlaceOrderService : ServiceErrors, IPlaceOrderService
    {
        private readonly IPlaceOrderDbAccess placeOrderDbAccess;
        private readonly ISignInContext signInContext;

        public PlaceOrderService(IPlaceOrderDbAccess placeOrderDbAccess,
            ISignInContext signInContext)
        {
            this.placeOrderDbAccess = placeOrderDbAccess;
            this.signInContext = signInContext;
        }

        public int PlaceOrder(PlaceOrderDto placeOrderDataIn)
        {
            var inputLines = placeOrderDataIn.Lines ?? Enumerable.Empty<PlaceOrderLineItemDto>();
            if (inputLines.Count() > DomainConstants.OrderLineItemsLimit)
            {
                AddError(
                    errorMessage: $"order line items limit exceeded ({DomainConstants.OrderLineItemsLimit})");
                return 0;
            }
            var chosenIds = inputLines
                .Select(l => l.BookId)
                .ToArray();
            if (!chosenIds.Any())
            {
                AddError(errorMessage: "No items in your order.");
                return 0;
            }
            if (!PerformValidationObjectProperties(instance: placeOrderDataIn))
            {
                return 0;
            }
            Dictionary<Guid, Book> booksDict = placeOrderDbAccess
                .FindBooksByIds(bookIds: chosenIds);

            var orderLines = FormOrderLineItems(
                placeOrderLineItems: placeOrderDataIn.Lines,
                booksBaselineDict: booksDict);
            if (HasErrors)
            {
                return 0;
            }
            string userId = signInContext.IsSignedIn
                ? signInContext.UserId
                : null;
            Order order = new Order
            {
                DateOrderedUtc = DateTime.UtcNow,
                Firstname = placeOrderDataIn.Firstname,
                LastName = placeOrderDataIn.Lastname,
                PhoneNumber = placeOrderDataIn.PhoneNumber,
                Lines = orderLines,
                UserId = userId
            };
            placeOrderDbAccess.Add(newOrder: order);
            placeOrderDbAccess.SaveChanges();
            return order.OrderId;
        }

        private IList<OrderLineItem> FormOrderLineItems(
            IEnumerable<PlaceOrderLineItemDto> placeOrderLineItems,
            Dictionary<Guid, Book> booksBaselineDict)
        {
            List<OrderLineItem> orderLines = new List<OrderLineItem>();
            foreach (var line in placeOrderLineItems)
            {
                if (!PerformValidationObjectProperties(instance: line))
                {
                    break;
                }
                if (!booksBaselineDict.ContainsKey(line.BookId))
                {
                    throw new InvalidOperationException(
                        message: $"A placing of order failed: book id={line.BookId} was missing");
                }
                var baselineBook = booksBaselineDict[line.BookId];
                if (line.Price != baselineBook.Price)
                {
                    AddError(errorMessage: "items have expired price");
                    break;
                }
                orderLines.Add(new OrderLineItem
                {
                    Book = baselineBook,
                    BookId = baselineBook.BookId,
                    BookPrice = baselineBook.Price,
                    Quantity = line.Quantity
                });
            }
            return orderLines;
        }


    }
}
