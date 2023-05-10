using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Entities;

namespace BookApp.BLL.Services.OrderProcessing
{
    public interface IOrderProcessingDbAccess
    {
        IEnumerable<OrderItemDto> GetOrders();
        OrderDetailsDto GetOrder(int orderId);
        Order GetOrderOrigin(int orderId);
        IEnumerable<OrderLineItem> GetOrderLines(int orderId);
        void SaveOrder(Order order);
    }
}
