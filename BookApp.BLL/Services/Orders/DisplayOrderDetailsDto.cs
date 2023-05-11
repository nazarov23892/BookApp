using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Entities;

namespace BookApp.BLL.Services.Orders
{
    public class DisplayOrderDetailsDto
    {
        public int OrderId { get; set; }
        public DateTime DateOrderedUtc { get; set; }
        public OrderStatus Status { get; set; }
        public IEnumerable<DisplayOrderDetailsLineItemDto> Lines { get; set; }
    }

    public class DisplayOrderDetailsLineItemDto
    {
        public Guid BookId { get; set; }
        public string BookTItle { get; set; }
        public decimal BookPrice { get; set; }
        public int Quantity { get; set; }
    }
}
