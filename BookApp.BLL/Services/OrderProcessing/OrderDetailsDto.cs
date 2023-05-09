using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Entities;

namespace BookApp.BLL.Services.OrderProcessing
{
    public class OrderDetailsDto
    {
        public int OrderId { get; set; }
        public DateTime DateOrderedUtc { get; set; }
        public OrderStatus Status { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<OrderDetailsLineDto> Lines { get; set; }
    }

    public class OrderDetailsLineDto
    {
        public Guid BookId { get; set; }
        public string BookTitle { get; set; }
        public decimal BookPrice { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<OrderDetailsAuthorDto> Authors { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderDetailsAuthorDto
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
