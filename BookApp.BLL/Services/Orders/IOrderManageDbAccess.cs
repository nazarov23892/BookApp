using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Entities;

namespace BookApp.BLL.Services.Orders
{
    public interface IOrderManageDbAccess
    {
        Order GetOrder(int orderId);
        void SaveOrder(Order order);
    }
}
