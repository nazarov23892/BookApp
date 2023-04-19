using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.Abstract;
using Domain.Entities;
using BookApp.BLL.Interfaces;

namespace ServiceLayer.OrderServices.Concrete
{
    public class DisplayOrderService : ServiceErrors, IDisplayOrderService
    {
        //private readonly AppIdentityDbContext efDbContext;
        private readonly ISignInContext signInContext;
        private readonly IDisplayOrderDbAccess displayOrderDbAccess;

        public DisplayOrderService(
            IDisplayOrderDbAccess displayOrderDbAccess,
            //AppIdentityDbContext efDbContext,
            ISignInContext signInContext)
        {
            this.displayOrderDbAccess = displayOrderDbAccess;
            //this.efDbContext = efDbContext;
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
