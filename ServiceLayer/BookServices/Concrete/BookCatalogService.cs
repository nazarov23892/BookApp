using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DataContexts;

namespace ServiceLayer.BookServices.Concrete
{
    public class BookCatalogService : IBookCatalogService
    {
        private AppIdentityDbContext efDbContext;

        public BookCatalogService(AppIdentityDbContext efDbContext)
        {
            this.efDbContext = efDbContext;
        }

        public IEnumerable<Book> GetList()
        {
            return efDbContext.Books
                .OrderBy(b=>b.Title)
                .ToArray();
        }
    }
}
