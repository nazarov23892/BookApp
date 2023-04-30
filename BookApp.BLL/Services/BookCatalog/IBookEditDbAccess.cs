using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Entities;

namespace BookApp.BLL.Services.BookCatalog
{
    public interface IBookEditDbAccess
    {
        BookAuthorsLinkOrderDto GetBookForEditAuthors(Guid bookId);
        IEnumerable<BookAuthorsLinkOrderItemDto> GetAuthors();
        Dictionary<Guid, Author> GetAuthorsByIds(IEnumerable<Guid> authorIds);
        Book GetBookWithAuthorLinks(Guid bookId);
        BookAuthorsToAddDto GetAuthorsForAdd(Guid bookId);
        Author GetAuthor(Guid authorId);
        void SaveBook(Book book);
    }
}
