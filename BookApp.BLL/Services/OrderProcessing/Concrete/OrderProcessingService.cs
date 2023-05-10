using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Generic;
using BookApp.BLL.Entities;

namespace BookApp.BLL.Services.OrderProcessing.Concrete
{
    public class OrderProcessingService : ServiceErrors, IOrderProcessingService
    {
        private readonly IOrderProcessingDbAccess orderProcessingDbAccess;

        public OrderProcessingService(IOrderProcessingDbAccess orderProcessingDbAccess)
        {
            this.orderProcessingDbAccess = orderProcessingDbAccess;
        }

        public OrderDetailsDto GetOrder(int orderId)
        {
            OrderDetailsDto orderDto = orderProcessingDbAccess.GetOrder(orderId);
            if (orderDto == null)
            {
                return null;
            }
            orderDto.IsAssemblingStatusAble = orderDto.Status == OrderStatus.New;
            return orderDto;
        }

        public OrderDetailsDto GetOrderForAssembling(int orderId)
        {
            OrderDetailsDto orderDetailsDto = orderProcessingDbAccess.GetOrder(orderId);
            if (orderDetailsDto == null)
            {
                return null;
            }
            if (orderDetailsDto.Status != OrderStatus.Assembling)
            {
                return null;
            }
            return orderDetailsDto;
        }

        public void SetOrderStatusToAssembling(int orderId)
        {
            Order order = orderProcessingDbAccess.GetOrderOrigin(orderId);
            if (order == null)
            {
                AddError("order not found");
                return;
            }
            if (order.Status != OrderStatus.New)
            {
                AddError("can assembly new orders only");
                return;
            }
            order.Status = OrderStatus.Assembling;
            orderProcessingDbAccess.SaveOrder(order);
        }
    }
}
