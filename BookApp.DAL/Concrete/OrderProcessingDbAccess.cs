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
                .OrderBy(o=>o.DateOrderedUtc)
                .Select(o=>new OrderItemDto
                {
                     OrderId = o.OrderId,
                     DateOrderedUtc = o.DateOrderedUtc,
                     Status = o.Status
                })
                .ToList();
        }
    }
}
