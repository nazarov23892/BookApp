using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DataContexts;
using Microsoft.EntityFrameworkCore;
using DataLayer.Concrete.QueryObjects;
using BookApp.BLL.Services.BookCatalog;

namespace DataLayer.Concrete
{
    public class BookCatalogDbAccess : BookApp.BLL.Services.BookCatalog.IBookCatalogDbAccess
    {
        private readonly AppIdentityDbContext efDbContext;

        public BookCatalogDbAccess(AppIdentityDbContext efDbContext)
        {
            this.efDbContext = efDbContext;
        }

        public int GetCount()
        {
            return efDbContext.Books.Count();
        }

        public BookDetailsDto GetItem(Guid bookId)
        {
            return efDbContext.Books
                .AsNoTracking()
                .MapToBookDetailsDto()
                .SingleOrDefault(b => b.BookId == bookId);
        }

        public IEnumerable<BookCatalogDto> GetList(PageOptionsIn pageOptionsIn)
        {
            return efDbContext.Books
            .AsNoTracking()
            .MapToBookCatalogDto()
            .OrderBy(b => b.Title)
            .Paging(
                pageNumZeroStart: pageOptionsIn.Page - 1,
                pageSize: pageOptionsIn.PageSize)
            .ToArray();
        }
    }
}
