using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderLineItem
    {
        public int OrderLineItemId { get; set; }
        public int OrderId { get; set; }
        public Guid BookId { get; set; }
        public decimal BookPrice { get; set; }
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}
