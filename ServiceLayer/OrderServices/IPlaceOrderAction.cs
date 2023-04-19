using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.Shared.DTOs.Orders;
using ServiceLayer.Abstract;

namespace ServiceLayer.OrderServices
{
    public interface IPlaceOrderAction : IServiceErrors
    {
        public int Run(PlaceOrderDto orderDto);
    }
}
