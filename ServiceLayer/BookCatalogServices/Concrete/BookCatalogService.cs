
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServiceLayer.BookCatalogServices.QueryObjects;

namespace ServiceLayer.BookCatalogServices.Concrete
{
    public class BookCatalogService : IBookCatalogService
    {
        //private AppIdentityDbContext efDbContext;
        private readonly IBookCatalogDbAccess bookCatalogDbAccess;

        public BookCatalogService(
            //AppIdentityDbContext efDbContext,
            IBookCatalogDbAccess bookCatalogDbAccess)
        {
            //this.efDbContext = efDbContext;
            this.bookCatalogDbAccess = bookCatalogDbAccess;
        }

        public BookDetailsDto GetItem(Guid bookId)
        {
            BookDetailsDto book = null;
                //efDbContext.Books
                //.AsNoTracking()
                //.MapToBookDetailsDto()
                //.SingleOrDefault(b => b.BookId == bookId);
            return book;
        }

        public BookListCombinedDto GetList(PageOptionsIn pageOptionsIn)
        {
            int booksNum = bookCatalogDbAccess.GetCount();
                //efDbContext.Books.Count();
            int pageSize = pageOptionsIn.PageSize;
            int pageCount = (booksNum / pageSize)
                + (booksNum % pageSize > 0 ? 1 : 0);

            var books = bookCatalogDbAccess.GetList(pageOptionsIn);


                //efDbContext.Books
                //.AsNoTracking()
                //.MapToBookCatalogDto()
                //.OrderBy(b=>b.Title)
                //.Paging(
                //    pageNumZeroStart: pageOptionsIn.Page - 1,
                //    pageSize: pageOptionsIn.PageSize)
                //.ToArray();

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
