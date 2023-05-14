using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Services.BookCatalog;

namespace BookApp.DAL.Concrete.QueryObjects
{
    public static class BookOrder
    {
        public static IQueryable<BookCatalogDto> OrderBooksBy(this IQueryable<BookCatalogDto> query, PageSortOptions sortOptions)
        {
            switch (sortOptions)
            {
                case PageSortOptions.SimpleOrder:
                    return query.OrderBy(b => b.BookId);
                case PageSortOptions.ByPriceDesc:
                    return query.OrderByDescending(b => b.Price);
                case PageSortOptions.ByPriceAsc:
                    return query.OrderBy(b => b.Price);
                default:
                    throw new InvalidOperationException(message: $"invalid value {nameof(sortOptions)} = {sortOptions}");
            }
        }
    }
}
