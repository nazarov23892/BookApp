using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookApp.BLL.Services.BookCatalog;
using BookApp.DAL.DataContexts;
using BookApp.BLL.Entities;

namespace BookApp.DAL.Concrete
{
    public class BookEditDbAccess : IBookEditDbAccess
    {
        private readonly AppIdentityDbContext efDbContext;

        public BookEditDbAccess(AppIdentityDbContext efDbContext)
        {
            this.efDbContext = efDbContext;
        }

        public Dictionary<Guid, Author> GetAuthorsByIds(IEnumerable<Guid> authorIds)
        {
            return efDbContext.Authors
                .Where(a => authorIds.Contains(a.AuthorId))
                .ToDictionary(a => a.AuthorId); 
        }

        public IEnumerable<BookEditAuthorsItemAuthorDto> GetAuthors()
        {
            IEnumerable<BookEditAuthorsItemAuthorDto> authors = efDbContext.Authors
                .AsNoTracking()
                .Select(a => new BookEditAuthorsItemAuthorDto
                {
                    AuthorId = a.AuthorId,
                    Firstname = a.Firstname,
                    Lastname = a.Lastname
                })
                .ToArray();
            return authors;
        }

        public BookEditAuthorsDto GetBookForEditAuthors(Guid bookId)
        {
            BookEditAuthorsDto book = efDbContext.Books
                .AsNoTracking()
                .Select(b => new BookEditAuthorsDto
                {
                    BookId = b.BookId,
                    BookTitle = b.Title,
                    ChosenAuthorsIds = b.AuthorsLink
                        .OrderBy(al => al.Order)
                        .Select(al => new BookEditAuthorsItemAuthorDto
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
