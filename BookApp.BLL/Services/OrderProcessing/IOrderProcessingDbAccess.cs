using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.OrderProcessing
{
    public interface IOrderProcessingDbAccess
    {
        IEnumerable<OrderItemDto> GetOrders();
        OrderDetailsDto GetOrder(int orderId);
    }
}
