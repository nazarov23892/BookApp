using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.Orders
{
    public class DisplayListOrderItemDto
    {
        public int OrderId { get; set; }
        public DateTime DateOrderedUtc { get; set; }
        public decimal Price { get; set; }
    }
}
