﻿using System;
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

        public void SetOrderStatusToReady(OrderAssemblingCompletedDto orderAssemblingDto)
        {
            bool hasLinesNotIncluded = orderAssemblingDto.LineItems
                .Where(b => !b.Included)
                .Any();
            if (hasLinesNotIncluded)
            {
                AddError("not all items included");
                return;
            }
            Order order = orderProcessingDbAccess.GetOrderOrigin(orderId: orderAssemblingDto.OrderId);
            if (order == null)
            {
                AddError($"order not found id='{orderAssemblingDto.OrderId}'");
                return;
            }
            var orderBooks = orderAssemblingDto.LineItems
                .Select(l => l.BookId);
            var orderItemsBaseline = orderProcessingDbAccess.GetOrderLines(orderId: orderAssemblingDto.OrderId);
            var orderBooksBaseline = orderItemsBaseline.Select(ol => ol.BookId);

            bool isLinesMatch = orderBooks.Count() == orderBooksBaseline.Count()
                && orderBooksBaseline.Intersect(orderBooks).Count() == orderBooksBaseline.Count();
            if (!isLinesMatch)
            {
                AddError($"lines contain missing item");
                return;
            }
            order.Status = OrderStatus.Ready;
            orderProcessingDbAccess.SaveOrder(order);
        }
    }
}
