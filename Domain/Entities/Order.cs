using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime DateOrderedUtc { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public ICollection<OrderLineItem> Lines { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
