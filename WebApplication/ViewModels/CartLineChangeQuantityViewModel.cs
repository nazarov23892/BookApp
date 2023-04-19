using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Domain;

namespace WebApplication.ViewModels
{
    public class CartLineChangeQuantityViewModel
    {
        [Required]
        public Guid BookId { get; set; }

        [Required]
        [Range(minimum: 1, maximum: DomainConstants.MaxQuantityToBuy)]
        public int Quantity { get; set; }
    }
}
