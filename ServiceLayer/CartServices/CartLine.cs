using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CartServices
{
    public class CartLine
    {
        public BookForCartDto Book { get; set; }
        public int Quantity { get; set; }
    }
}
