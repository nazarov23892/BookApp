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

        public BookEditService(IBookCatalogDbAccess bookCatalogDbAccess)
        {
            this.bookCatalogDbAccess = bookCatalogDbAccess;
        }

        public Guid CreateBook(BookCreateDto newBook)
        {
            return this.bookCatalogDbAccess.Create(newBook);
        }
    }
}