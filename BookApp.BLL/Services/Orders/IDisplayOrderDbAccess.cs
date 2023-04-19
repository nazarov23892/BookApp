using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.Orders
{
    public interface IDisplayOrderDbAccess
    {
        DisplayOrderDetailsDto GetItem(int orderId, string userId);
        IEnumerable<DisplayListOrderItemDto> GetItems(string userId);
    }
}
