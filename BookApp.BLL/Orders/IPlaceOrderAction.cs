using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using BookApp.Shared.DTOs.Orders;
using BookApp.BLL.Generic;

namespace BookApp.BLL.Orders
{
    public interface IPlaceOrderAction : IBizErrors
    {
        public int Run(PlaceOrderDto orderDto);
    }
}
