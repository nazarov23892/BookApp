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

        public Dictionary<Guid, Author> GetAuthorsByIds(IEnumerable<Guid> authorIds)
        {
            return efDbContext.Authors
                .Where(a => authorIds.Contains(a.AuthorId))
                .ToDictionary(a => a.AuthorId);
        }

        public IEnumerable<BookAuthorsLinkOrderItemDto> GetAuthors()
        {
            IEnumerable<BookAuthorsLinkOrderItemDto> authors = efDbContext.Authors
                .AsNoTracking()
                .Select(a => new BookAuthorsLinkOrderItemDto
                {
                    AuthorId = a.AuthorId,
                    Firstname = a.Firstname,
                    Lastname = a.Lastname
                })
                .ToArray();
            return authors;
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

        public Author GetAuthor(Guid authorId)
        {
            return efDbContext.Authors
                .SingleOrDefault(a => a.AuthorId == authorId);
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
                .Select(b=>new BookTagsForAddDto
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
    }
}
