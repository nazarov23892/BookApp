using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Entities;

namespace BookApp.BLL.Services.OrderProcessing
{
    public class OrderItemDto
    {
        public int OrderId { get; set; }
        public DateTime DateOrderedUtc { get; set; }
        public OrderStatus Status { get; set; }
    }
}
