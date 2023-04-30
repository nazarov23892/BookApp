using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.BookCatalog
{
    public interface IBookCatalogDbAccess
    {
        BookDetailsDto GetItem(Guid bookId);
        IEnumerable<BookCatalogDto> GetList(PageOptionsIn pageOptionsIn);
        int GetCount();
    }
}
