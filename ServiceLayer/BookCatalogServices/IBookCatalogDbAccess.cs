using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.BookCatalogServices
{
    public interface IBookCatalogDbAccess
    {
        BookDetailsDto GetItem(Guid bookId);
        IEnumerable<BookCatalogDto> GetList(PageOptionsIn pageOptionsIn);
        int GetCount();
    }
}
