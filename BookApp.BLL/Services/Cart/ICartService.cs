using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Generic;

namespace BookApp.BLL.Services.Cart
{
    public interface ICartService : IServiceErrors
    {
        IEnumerable<CartLine> Lines { get; }

        void Add(BookForCartDto book);

        void SetQuantity(Guid bookId, int quantity);

        void Remove(Guid bookId);

        void Clear();
    }
}
