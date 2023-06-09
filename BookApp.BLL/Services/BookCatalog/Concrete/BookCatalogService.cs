﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.BookCatalog.Concrete
{
    public class BookCatalogService : IBookCatalogService
    {
        private readonly IBookCatalogDbAccess bookCatalogDbAccess;

        public BookCatalogService(IBookCatalogDbAccess bookCatalogDbAccess)
        {
            this.bookCatalogDbAccess = bookCatalogDbAccess;
        }

        public BookDetailsDto GetItem(Guid bookId)
        {
            BookDetailsDto book = bookCatalogDbAccess.GetItem(bookId);
            return book;
        }

        public BookListCombinedDto GetList(PageOptionsIn pageOptionsIn)
        {
            BookCatalogListDto books = bookCatalogDbAccess.GetList(pageOptionsIn);
            int pageSize = pageOptionsIn.PageSize;
            int pageCount = (books.TotalCount / pageSize)
                + (books.TotalCount % pageSize > 0 ? 1 : 0);

            var tags = bookCatalogDbAccess.GetTags();
            var result = new BookListCombinedDto
            {
                PageOptionsOut = new PageOptionsOut
                {
                    Page = pageOptionsIn.Page,
                    PageSize = pageOptionsIn.PageSize,
                    PageCount = pageCount,
                    SortOption = pageOptionsIn.SortOption,
                    FilterTag = pageOptionsIn.FilterTag
                },
                Books = books.Items,
                Tags = tags
            };
            return result;
        }
    }
}
