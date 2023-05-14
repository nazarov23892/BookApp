using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookApp.BLL.Entities;
using BookApp.BLL.Services.BookCatalog;
using BookApp.DAL.DataContexts;
using BookApp.DAL.Concrete.QueryObjects;

namespace BookApp.DAL.Concrete
{
    public class BookCatalogDbAccess : IBookCatalogDbAccess
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
            .FilterByTag(pageOptionsIn.FilterTag)
            .MapToBookCatalogDto()
            .OrderBooksBy(pageOptionsIn.SortOption)
            .Paging(
                pageNumZeroStart: pageOptionsIn.Page - 1,
                pageSize: pageOptionsIn.PageSize)
            .ToArray();
        }

        public IEnumerable<BookListTagDto> GetTags()
        {
            return efDbContext.Set<Tag>()
                .AsNoTracking()
                .Select(t => new BookListTagDto
                {
                    TagId = t.TagId,
                    TagText = t.Text
                })
                .OrderBy(t=>t.TagText)
                .ToList();
        }
    }
}
