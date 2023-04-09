using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.Abstract;

namespace ServiceLayer.CartServices
{
    public interface ICartService: IServiceErrors
    {
        IEnumerable<CartLine> Lines { get; }

        void Add(BookForCartDto book);

        void SetQuantity(Guid bookId, int quantity);

        void Remove(Guid bookId);
        
        void Clear();
    }
}
