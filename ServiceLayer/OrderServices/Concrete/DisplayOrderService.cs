using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.Abstract;
using DataLayer.DataContexts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer.OrderServices.Concrete
{
    public class DisplayOrderService : ServiceErrors, IDisplayOrderService
    {
        private readonly AppIdentityDbContext efDbContext;
        private readonly ISignInContext signInContext;

        public DisplayOrderService(
            AppIdentityDbContext efDbContext,
            ISignInContext signInContext)
        {
            this.efDbContext = efDbContext;
            this.signInContext = signInContext;
        }

        public DisplayOrderDetailsDto GetItem(int orderId)
        {
            var userId = signInContext.IsSignedIn
               ? signInContext.UserId
               : null;
            if (string.IsNullOrEmpty(userId))
            {
                AddError(errorMessage: "cannot get orders for unauthorized users");
                return null;
            }
            var orderDto = efDbContext.Orders
                .AsNoTracking()
                .Where(o => o.UserId.Equals(userId))
                .Select(o => new DisplayOrderDetailsDto
                {
                    OrderId = o.OrderId,
                    DateOrderedUtc = o.DateOrderedUtc,
                    Lines = o.Lines
                        .Select(l => new DisplayOrderDetailsLineItemDto
                        {
                            BookId = l.BookId,
                            BookTItle = l.Book.Title,
                            BookPrice = l.BookPrice,
                            Quantity = l.Quantity
                        })
                })
                .SingleOrDefault(o => o.OrderId == orderId);
            return orderDto;
        }

        public IEnumerable<DisplayListOrderItemDto> GetOrders()
        {
            var userId = signInContext.IsSignedIn
                ? signInContext.UserId
                : null;
            if (string.IsNullOrEmpty(userId))
            {
                AddError(errorMessage: "cannot get orders for unauthorized users");
                return Enumerable.Empty<DisplayListOrderItemDto>();
            }
            var orders = efDbContext.Orders
                .AsNoTracking()
                .Where(o => o.UserId == userId)
                .Select(o => new DisplayListOrderItemDto
                {
                    OrderId = o.OrderId,
                    DateOrderedUtc = o.DateOrderedUtc,
                    Price = o.Lines
                        .Select(l => l.BookPrice * l.Quantity)
                        .Sum()
                });
            return orders;
        }
    }
}
