using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.OrderServices
{
    public interface IDisplayOrderDbAccess
    {
        DisplayOrderDetailsDto GetItem(int orderId, string userId);
        IEnumerable<DisplayListOrderItemDto> GetItems(string userId);
    }
}
