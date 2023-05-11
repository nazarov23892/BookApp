using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Generic;
using BookApp.BLL.Interfaces;
using BookApp.BLL.Entities;

namespace BookApp.BLL.Services.Orders.Concrete
{
    public class DisplayOrderService : ServiceErrors, IDisplayOrderService
    {
        private readonly ISignInContext signInContext;
        private readonly IDisplayOrderDbAccess displayOrderDbAccess;

        public DisplayOrderService(
            IDisplayOrderDbAccess displayOrderDbAccess,
            ISignInContext signInContext)
        {
            this.displayOrderDbAccess = displayOrderDbAccess;
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
            var orderDto = displayOrderDbAccess.GetItem(
                orderId: orderId,
                userId: userId);
            orderDto.IsCancelAble = orderDto.Status != OrderStatus.Completed
                && orderDto.Status != OrderStatus.Cancelled;
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
            IEnumerable<DisplayListOrderItemDto> orders = displayOrderDbAccess.GetItems(userId);
            return orders;
        }
    }
}
