using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataLayer.Entities;

namespace ServiceLayer.BookServices.QueryObjects
{
    public static class BookListPaging
    {
        public static IQueryable<Book> Paging(
            this IQueryable<Book> query, 
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
