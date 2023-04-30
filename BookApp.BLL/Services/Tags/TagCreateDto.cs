using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookApp.BLL.Services.Tags
{
    public class TagCreateDto
    {
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string Text { get; set; }
    }
}
