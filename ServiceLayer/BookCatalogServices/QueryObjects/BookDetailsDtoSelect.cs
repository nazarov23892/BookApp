using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer.BookCatalogServices.QueryObjects
{
    public static class BookDetailsDtoSelect
    {
        public static IQueryable<BookDetailsDto> MapToBookDetailsDto(this IQueryable<Book> query)
        {
            return query.Select(b => new BookDetailsDto
            {
                BookId = b.BookId,
                Title = b.Title,
                Price = b.Price,
                Authors = b.AuthorsLink
                    .OrderBy(al => al.Order)
                    .Select(al => new AuthorNameDto
                    {
                        FirstName = al.Author.Firstname,
                        LastName = al.Author.Lastname
                    })
            });
        }
    }
}
