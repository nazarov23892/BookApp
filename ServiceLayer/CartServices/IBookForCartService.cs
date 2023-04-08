using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CartServices
{
    public interface IBookForCartService
    {
        BookForCartDto GetItem(Guid bookId);
    }
}
