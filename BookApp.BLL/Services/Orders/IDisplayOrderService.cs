using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Generic;

namespace BookApp.BLL.Services.Orders
{
    public interface IDisplayOrderService : IServiceErrors
    {
        IEnumerable<DisplayListOrderItemDto> GetOrders();
        public DisplayOrderDetailsDto GetItem(int orderId);
    }
}
