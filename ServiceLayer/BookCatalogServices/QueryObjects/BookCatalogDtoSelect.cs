﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataLayer.Entities;

namespace ServiceLayer.BookCatalogServices.QueryObjects
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