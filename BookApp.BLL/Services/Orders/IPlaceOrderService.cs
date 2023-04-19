using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Generic;

namespace BookApp.BLL.Services.Orders
{
    public interface IPlaceOrderService : IServiceErrors
    {
        int PlaceOrder(PlaceOrderDto placeOrderDataIn);
    }
}
