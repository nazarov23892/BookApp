using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DataContexts;
using ServiceLayer.BookServices.QueryObjects;

namespace ServiceLayer.BookServices.Concrete
{
    public class BookCatalogService : IBookCatalogService
    {
        private AppIdentityDbContext efDbContext;

        public BookCatalogService(AppIdentityDbContext efDbContext)
        {
            this.efDbContext = efDbContext;
        }

        public BookListCombinedDto GetList(PageOptionsIn pageOptionsIn)
        {
            int booksNum = efDbContext.Books.Count();
            int pageSize = pageOptionsIn.PageSize;
            int pageCount = (booksNum / pageSize)
                + (booksNum % pageSize > 0 ? 1 : 0);

            var books = efDbContext.Books
                .OrderBy(b=>b.BookId)
                .Paging(
                    pageNumZeroStart: pageOptionsIn.Page - 1,
                    pageSize: pageOptionsIn.PageSize)
                .ToArray();

            var result = new BookListCombinedDto
            {
                PageOptionsOut = new PageOptionsOut
                {
                    Page = pageOptionsIn.Page,
                    PageSize = pageOptionsIn.PageSize,
                    PageCount = pageCount
                },
                Books = books
            };
            return result;
        }
    }
}
