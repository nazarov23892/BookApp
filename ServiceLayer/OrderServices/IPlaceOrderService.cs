using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.Abstract;

namespace ServiceLayer.OrderServices
{
    public interface IPlaceOrderService: IServiceErrors
    {
        int PlaceOrder(PlaceOrderDto placeOrderDataIn);
    }
}
