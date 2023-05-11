using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Generic;
using BookApp.BLL.Entities;
using BookApp.BLL.Interfaces;

namespace BookApp.BLL.Services.Orders.Concrete
{
    public class OrderManageService : ServiceErrors, IOrderManageService
    {
        private readonly IOrderManageDbAccess orderManageDbAccess;
        private readonly ISignInContext signInContext;

        public OrderManageService(
            IOrderManageDbAccess orderManageDbAccess,
            ISignInContext signInContext)
        {
            this.orderManageDbAccess = orderManageDbAccess;
            this.signInContext = signInContext;
        }

        public void Cancel(int orderId)
        {
            if (!signInContext.IsSignedIn)
            {
                AddError(errorMessage: "cannot get orders for unauthorized users");
                return;
            }
            Order order = orderManageDbAccess.GetOrder(orderId);
            if (order == null)
            {
                AddError(errorMessage: "order not found");
                return;
            }
            string userId = signInContext.UserId;
            if (!string.Equals(
                a: order.UserId,
                b: userId,
                comparisonType: StringComparison.OrdinalIgnoreCase))
            {
                AddError(errorMessage: "cannot cancel non-own order");
                return;
            }
            if (order.Status == OrderStatus.Cancelled)
            {
                AddError(errorMessage: "cannot cancel cancelled order");
                return;
            }
            if (order.Status == OrderStatus.Completed)
            {
                AddError(errorMessage: "cannot cancel completed order");
                return;
            }
            order.Status = OrderStatus.Cancelled;
            orderManageDbAccess.SaveOrder(order);
        }
    }
}
