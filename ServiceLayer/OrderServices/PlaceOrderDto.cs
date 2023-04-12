﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Domain;

namespace ServiceLayer.OrderServices
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
        public decimal Price { get; set; }
        
        [Range(minimum: 1, maximum: DomainConstants.MaxQuantityToBuy)]
        public int Quantity { get; set; }
    }
}
