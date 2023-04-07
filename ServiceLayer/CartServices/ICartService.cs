using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CartServices
{
    public interface ICartService
    {
        IEnumerable<CartLine> Lines { get; }

        void Add(BookForCartDto book);

        void SetQuantity(BookForCartDto book, int quantity);

        void Remove(BookForCartDto book);
        
        void Clear();
    }
}
