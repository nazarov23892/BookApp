using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.Cart
{
    public interface IBookForCartDbAccess
    {
        BookForCartDto GetItem(Guid bookId);
    }
}
