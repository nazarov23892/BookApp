using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace ServiceLayer.CartServices.QueryObjects
{
    public static class BookForCartDtoSelect
    {
        public static IQueryable<BookForCartDto> MapBookToBookForCartDto(
            this IQueryable<Book> query)
        {
            return query.Select(b => new BookForCartDto
            {
                BookId = b.BookId,
                Title = b.Title,
                Price = b.Price
            });
        }
    }

}
