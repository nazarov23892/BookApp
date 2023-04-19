using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using BookApp.BLL.Services.BookCatalog;

namespace DataLayer.Concrete.QueryObjects
{
    public static class BookListPaging
    {
        public static IQueryable<BookCatalogDto> Paging(
            this IQueryable<BookCatalogDto> query,
            int pageNumZeroStart,
            int pageSize)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(pageSize),
                    message: "cannot be less or equal zero.");
            }
            return query
                .Skip(pageNumZeroStart * pageSize)
                .Take(pageSize);
        }
    }
}
