using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookApp.BLL.Entities
{
    public class Tag
    {
        public int TagId { get; set; }

        [Required]
        [MaxLength(length: 40)]
        public string Text { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
