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
        BookEditAuthorsDto GetBookForEditAuthors(Guid bookId);
        IEnumerable<BookEditAuthorsItemAuthorDto> GetAuthors();
    }
}
