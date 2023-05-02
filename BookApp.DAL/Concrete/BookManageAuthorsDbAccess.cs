using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookApp.BLL.Entities;
using BookApp.BLL.Services.BookManageAuthors;
using BookApp.DAL.DataContexts;

namespace BookApp.DAL.Concrete
{
    public class BookManageAuthorsDbAccess : IBookManageAuthorsDbAccess
    {
        private readonly AppIdentityDbContext efDbContext;

        public BookManageAuthorsDbAccess(AppIdentityDbContext efDbContext)
        {
            this.efDbContext = efDbContext;
        }

        public Author GetAuthor(Guid authorId)
        {
            return efDbContext.Authors
                .SingleOrDefault(a => a.AuthorId == authorId);
        }

        public BookAuthorsToAddDto GetAuthorsForAdd(Guid bookId)
        {
            BookAuthorsToAddDto dto = efDbContext.Books
                .Select(b => new BookAuthorsToAddDto
                {
                    BookId = b.BookId,
                    BookTitle = b.Title
                })
                .SingleOrDefault(b => b.BookId == bookId);
            if (dto == null)
            {
                return null;
            }
            dto.Authors = efDbContext.Authors
                .Where(a => !a.BooksLink
                    .Where(l => l.BookId == bookId)
                    .Any())
                .Select(a => new BookAuthorsToAddItemDto
                {
                    AuthorId = a.AuthorId,
                    Firstname = a.Firstname,
                    Lastname = a.Lastname
                })
                .ToArray();
            return dto;
        }

        public BookAuthorsLinkOrderDto GetBookForEditAuthors(Guid bookId)
        {
            BookAuthorsLinkOrderDto book = efDbContext.Books
                .AsNoTracking()
                .Select(b => new BookAuthorsLinkOrderDto
                {
                    BookId = b.BookId,
                    BookTitle = b.Title,
                    ChosenAuthorsIds = b.AuthorsLink
                        .OrderBy(al => al.Order)
                        .Select(al => new BookAuthorsLinkOrderItemDto
                        {
                            AuthorId = al.AuthorId,
                            Firstname = al.Author.Firstname,
                            Lastname = al.Author.Lastname,
                            OrderNo = al.Order
                        })
                        .ToArray()
                })
                .SingleOrDefault(b => b.BookId == bookId);
            return book;
        }

        public Book GetBookWithAuthorLinks(Guid bookId)
        {
            return efDbContext.Books
                .Include(b => b.AuthorsLink)
                .ThenInclude(al => al.Author)
                .SingleOrDefault(b => b.BookId == bookId);
        }

        public void SaveBook(Book book)
        {
            efDbContext.Books.Update(book);
            efDbContext.SaveChanges();
        }
    }
}
