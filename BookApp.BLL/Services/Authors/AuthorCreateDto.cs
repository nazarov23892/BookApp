using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookApp.BLL.Services.Authors
{
    public class AuthorCreateDto
    {
        [Required]
        [StringLength(maximumLength: 40, MinimumLength = 4)]
        public string Firstname { get; set; }

        [Required]
        [StringLength(maximumLength: 40, MinimumLength = 4)]
        public string Lastname { get; set; }
    }
}
