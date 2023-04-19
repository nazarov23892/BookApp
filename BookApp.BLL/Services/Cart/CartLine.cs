using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.Cart
{
    public class CartLine
    {
        public BookForCartDto Book { get; set; }
        public int Quantity { get; set; }
    }
}
