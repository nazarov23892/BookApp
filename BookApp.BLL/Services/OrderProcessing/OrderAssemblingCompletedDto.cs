using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.OrderProcessing
{
    public class OrderAssemblingCompletedDto
    {
        public int OrderId { get; set; }
        public IEnumerable<OrderAssemblyCompletedItemDto> LineItems { get; set; }
    }

    public class OrderAssemblyCompletedItemDto
    {
        public Guid BookId { get; set; }
        public bool Included { get; set; }
    }
}
