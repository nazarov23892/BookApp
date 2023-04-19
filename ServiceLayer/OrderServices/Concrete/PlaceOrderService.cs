using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ServiceLayer.Abstract;
using Domain.Entities;
using BookApp.Shared.DTOs.Orders;
using ServiceLayer.Interfaces;
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
            IPlaceOrderAction placeOrderAction = new PlaceOrderAction(
                placeOrderDbAccess: placeOrderDbAccess,
                signInContext: signInContext);

            var orderId = placeOrderAction.Run(orderDto: placeOrderDataIn);
            if (placeOrderAction.HasErrors)
            {
                foreach (var error in placeOrderAction.Errors)
                {
                    AddError(errorMessage: error.ErrorMessage);
                }
                return 0;
            }
            placeOrderDbAccess.SaveChanges();
            return orderId;
        }

    }
}
