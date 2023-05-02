using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Entities;

namespace BookApp.BLL.Services.BookManageAuthors
{
    public interface IBookManageAuthorsDbAccess
    {
        BookAuthorsLinkOrderDto GetBookForEditAuthors(Guid bookId);
        BookAuthorsToAddDto GetAuthorsForAdd(Guid bookId);
        Book GetBookWithAuthorLinks(Guid bookId);
        Author GetAuthor(Guid authorId);
        void SaveBook(Book book);
    }
}
