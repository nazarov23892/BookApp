using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Generic;

namespace BookApp.BLL.Services.BookCatalog.Concrete
{
    public class BookEditService : ServiceErrors, IBookEditService
    {
        private readonly IBookCatalogDbAccess bookCatalogDbAccess;
        private readonly IBookEditDbAccess bookEditDbAccess;

        public BookEditService(
            IBookCatalogDbAccess bookCatalogDbAccess,
            IBookEditDbAccess bookEditDbAccess)
        {
            this.bookCatalogDbAccess = bookCatalogDbAccess;
            this.bookEditDbAccess = bookEditDbAccess;
        }

        public Guid CreateBook(BookCreateDto newBook)
        {
            return this.bookCatalogDbAccess.Create(newBook);
        }

        public BookEditAuthorsCombinedDto GetBookForEditAuthors(Guid bookId)
        {
            IEnumerable<BookEditAuthorsItemAuthorDto> authors = bookEditDbAccess.GetAuthors();
            BookEditAuthorsDto book = bookEditDbAccess.GetBookForEditAuthors(bookId);
            return new BookEditAuthorsCombinedDto
            {
                Book = book,
                Authors = authors
            };
        }
    }
}