using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.Abstract;
using BookApp.Shared.DTOs.Orders;

namespace ServiceLayer.OrderServices
{
    public interface IPlaceOrderService: IServiceErrors
    {
        int PlaceOrder(PlaceOrderDto placeOrderDataIn);
    }
}
