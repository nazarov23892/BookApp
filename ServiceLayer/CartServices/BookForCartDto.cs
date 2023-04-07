using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CartServices
{
    public class BookForCartDto
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}
