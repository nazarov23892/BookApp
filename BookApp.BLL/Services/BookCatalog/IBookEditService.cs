using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Generic;

namespace BookApp.BLL.Services.BookCatalog
{
    public interface IBookEditService : IServiceErrors
    {
        Guid CreateBook(BookCreateDto newBook);
        BookEditAuthorsCombinedDto GetBookForEditAuthors(Guid bookId);
        void ChangeAuthorLinksOrder(BookAuthorLinksOrderEditedDto authorLinksDto);
    }
}