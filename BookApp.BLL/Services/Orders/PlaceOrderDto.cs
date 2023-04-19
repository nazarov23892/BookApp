using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using System.ComponentModel.DataAnnotations;

namespace BookApp.BLL.Services.Orders
{
    public class PlaceOrderDto
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        public IEnumerable<PlaceOrderLineItemDto> Lines { get; set; }
    }

    public class PlaceOrderLineItemDto
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }

        [Range(
            minimum: DomainConstants.MinPrice,
            maximum: DomainConstants.MaxPrice,
            ErrorMessage = "invalid price value")]
        public decimal Price { get; set; }

        [Range(minimum: 1, maximum: DomainConstants.MaxQuantityToBuy)]
        public int Quantity { get; set; }
    }
}
