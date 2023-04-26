using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookApp.BLL.Services.Orders;
using BookApp.DAL.DataContexts;

namespace BookApp.DAL.Concrete
{
    public class DisplayOrderDbAccess : IDisplayOrderDbAccess
    {
        private readonly AppIdentityDbContext efDbContext;

        public DisplayOrderDbAccess(AppIdentityDbContext efDbContext)
        {
            this.efDbContext = efDbContext;
        }

        public DisplayOrderDetailsDto GetItem(int orderId, string userId)
        {
            return efDbContext.Orders
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
        }

        public IEnumerable<DisplayListOrderItemDto> GetItems(string userId)
        {
            IEnumerable<DisplayListOrderItemDto> orders = efDbContext.Orders
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
