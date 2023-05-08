using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Services.BookCatalog;
using BookApp.BLL.Entities;

namespace BookApp.DAL.Concrete.QueryObjects
{
    public static class BookCatalogDtoSelect
    {
        public static IQueryable<BookCatalogDto> MapToBookCatalogDto(this IQueryable<Book> query)
        {
            return query.Select(b => new BookCatalogDto
            {
                BookId = b.BookId,
                Title = b.Title,
                Price = b.Price,
                ImageUrl = b.ImageUrl,
                Authors = b.AuthorsLink
                    .OrderBy(al => al.Order)
                    .Select(al => new AuthorNameDto
                    {
                        FirstName = al.Author.Firstname,
                        LastName = al.Author.Lastname
                    }),
                Tags = b.Tags.Select(t => t.Text)
            });
        }
    }
}
