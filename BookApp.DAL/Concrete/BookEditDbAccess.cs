using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookApp.BLL.Services.BookManage;
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

        public void SaveBook(Book book)
        {
            efDbContext.Books.Update(book);
            efDbContext.SaveChanges();
        }

        public BookWithTagsDto GetBookForEditTags(Guid bookId)
        {
            return efDbContext.Books
                .AsNoTracking()
                .Select(b => new BookWithTagsDto
                {
                    BookId = b.BookId,
                    BookTitle = b.Title,
                    Tags = b.Tags
                        .Select(t => new BookWithTagsItemDto
                        {
                            TagId = t.TagId,
                            Text = t.Text
                        })
                })
                .SingleOrDefault(b => b.BookId == bookId);
        }

        public BookTagsForAddDto GetTagsForAdd(Guid bookId)
        {
            BookTagsForAddDto book = efDbContext.Books
                .Select(b => new BookTagsForAddDto
                {
                    BookId = b.BookId,
                    BookTitle = b.Title
                })
                .SingleOrDefault(b => b.BookId == bookId);
            if (book == null)
            {
                return null;
            }
            var tags = efDbContext.Set<Tag>()
                .AsNoTracking()
                .Where(b => !b.Books.Where(b => b.BookId == bookId).Any())
                .Select(t => new BookTagsForAddItemDto
                {
                    TagId = t.TagId,
                    Text = t.Text
                });
            book.Tags = tags;
            return book;
        }

        public Book GetBookWithTags(Guid bookId)
        {
            return efDbContext.Books
                .Include(b => b.Tags)
                .SingleOrDefault(b => b.BookId == bookId);
        }

        public Tag GetTag(int tagId)
        {
            return efDbContext.Set<Tag>()
                .SingleOrDefault(t => t.TagId == tagId);
        }

        public Guid Create(BookCreateDto newBook)
        {
            var book = new Book
            {
                BookId = Guid.NewGuid(),
                Title = newBook.Title,
                Price = newBook.Price
            };
            efDbContext.Books.Add(book);
            efDbContext.SaveChanges();
            return book.BookId;
        }

        public BookDescriptionForEditDto GetBookForEditDescription(Guid bookId)
        {
            return efDbContext.Books
                .AsNoTracking()
                .Select(b => new BookDescriptionForEditDto
                {
                    BookId = b.BookId,
                    BookTitle = b.Title,
                    Description = b.Description
                })
                .SingleOrDefault(b => b.BookId == bookId);
        }

        public Book GetBook(Guid bookId)
        {
            return efDbContext.Books
                .SingleOrDefault(b => b.BookId == bookId);
        }
    }
}
