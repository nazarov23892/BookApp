using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookApp.BLL.Services.BookManage
{
    public class BookCreateDto
    {
        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [Range(minimum: DomainConstants.MinPrice, maximum: DomainConstants.MaxPrice)]
        public decimal Price { get; set; }
    }
}
