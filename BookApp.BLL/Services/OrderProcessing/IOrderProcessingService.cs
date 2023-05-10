using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Generic;

namespace BookApp.BLL.Services.OrderProcessing
{
    public interface IOrderProcessingService: IServiceErrors
    {
        OrderDetailsDto GetOrder(int orderId);
        OrderDetailsDto GetOrderForAssembling(int orderId);
        void SetOrderStatusToAssembling(int orderId);
    }
}
