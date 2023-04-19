using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.BookCatalog
{
    public interface IBookCatalogService
    {
        public BookListCombinedDto GetList(PageOptionsIn pageOptionsIn);

        public BookDetailsDto GetItem(Guid bookId);
    }
}
