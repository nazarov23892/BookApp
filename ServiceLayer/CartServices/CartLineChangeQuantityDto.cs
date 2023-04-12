using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Domain;

namespace ServiceLayer.CartServices
{
    public class CartLineChangeQuantityDto
    {
        [Required]
        public Guid BookId { get; set; }

        [Required]
        [Range(minimum: 1, maximum: DomainConstants.MaxQuantityToBuy)]
        public int Quantity { get; set; }
    }
}
