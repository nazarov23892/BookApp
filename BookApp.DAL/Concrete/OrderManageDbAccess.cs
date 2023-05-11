using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Entities;
using BookApp.BLL.Services.Orders;
using BookApp.DAL.DataContexts;

namespace BookApp.DAL.Concrete
{
    public class OrderManageDbAccess : IOrderManageDbAccess
    {
        private readonly AppIdentityDbContext efDbContext;

        public OrderManageDbAccess(AppIdentityDbContext efDbContext)
        {
            this.efDbContext = efDbContext;
        }

        public Order GetOrder(int orderId)
        {
            return efDbContext.Orders
                .SingleOrDefault(o => o.OrderId == orderId);
        }

        public void SaveOrder(Order order)
        {
            efDbContext.Orders.Update(order);
            efDbContext.SaveChanges();
        }
    }
}
