using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookApp.BLL.Entities;
using BookApp.BLL.Services.OrderProcessing;
using BookApp.DAL.DataContexts;

namespace BookApp.DAL.Concrete
{
    public class OrderProcessingDbAccess : IOrderProcessingDbAccess
    {
        private readonly AppIdentityDbContext efDbContext;

        public OrderProcessingDbAccess(AppIdentityDbContext efDbContext)
        {
            this.efDbContext = efDbContext;
        }

        public IEnumerable<OrderItemDto> GetOrders()
        {
            return efDbContext.Orders
                .AsNoTracking()
                .OrderBy(o => o.DateOrderedUtc)
                .Select(o => new OrderItemDto
                {
                    OrderId = o.OrderId,
                    DateOrderedUtc = o.DateOrderedUtc,
                    Status = o.Status
                })
                .ToList();
        }

        public OrderDetailsDto GetOrder(int orderId)
        {
            return efDbContext.Orders
                 .AsNoTracking()
                 .Select(o => new OrderDetailsDto
                 {
                     OrderId = o.OrderId,
                     DateOrderedUtc = o.DateOrderedUtc,
                     Firstname = o.Firstname,
                     LastName = o.LastName,
                     PhoneNumber = o.PhoneNumber,
                     Status = o.Status,
                     Lines = o.Lines
                        .Select(l => new OrderDetailsLineDto
                        {
                            BookId = l.BookId,
                            BookTitle = l.Book.Title,
                            BookPrice = l.BookPrice,
                            ImageUrl = l.Book.ImageUrl,
                            Quantity = l.Quantity,
                            Authors = null
                        })
                        .ToList()
                 })
                 .SingleOrDefault(o => o.OrderId == orderId);
        }

        public Order GetOrderOrigin(int orderId)
        {
            return efDbContext.Orders
                .SingleOrDefault(o => o.OrderId == orderId);
        }

        public void SaveOrder(Order order)
        {
            efDbContext.Orders
                .Update(order);
            efDbContext.SaveChanges();
        }

        public IEnumerable<OrderLineItem> GetOrderLines(int orderId)
        {
            return efDbContext.Set<OrderLineItem>()
                .AsNoTracking()
                .Where(ol => ol.OrderId == orderId)
                .ToList();
        }
    }
}
