using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Generic;
using BookApp.BLL.Entities;

namespace BookApp.BLL.Services.BookCatalog.Concrete
{
    public class BookEditService : ServiceErrors, IBookEditService
    {
        private readonly IBookCatalogDbAccess bookCatalogDbAccess;
        private readonly IBookEditDbAccess bookEditDbAccess;

        public BookEditService(
            IBookCatalogDbAccess bookCatalogDbAccess,
            IBookEditDbAccess bookEditDbAccess)
        {
            this.bookCatalogDbAccess = bookCatalogDbAccess;
            this.bookEditDbAccess = bookEditDbAccess;
        }

        public void AddAuthor(BookAddAuthorDto addAuthorDto)
        {
            // take book with authors
            // check book exist in db
            var book = bookEditDbAccess.GetBookWithAuthorLinks(bookId: addAuthorDto.BookId);
            if (book == null)
            {
                AddError(errorMessage: $"book id={addAuthorDto.BookId} not found");
                return;
            }
            BookAuthor[] bookAuthorsArray = book.AuthorsLink
                .ToArray();

            // check author exist in db
            var author = bookEditDbAccess.GetAuthor(authorId: addAuthorDto.AuthorId);
            if (author == null)
            {
                AddError(errorMessage: $"author id={addAuthorDto.AuthorId} not found");
                return;
            }

            // check author exist in book
            bool isAlreadyExist = bookAuthorsArray
                .Where(al => al.AuthorId == author.AuthorId)
                .Any();
            if (isAlreadyExist)
            {
                AddError(errorMessage: "book already contains given author");
                return;
            }
            int orderValue = bookAuthorsArray.Any()
                ? 1 + bookAuthorsArray.Max(al => al.Order)
                : 0;
            book.AuthorsLink.Add(new BookAuthor
            {
                Author = author,
                Order = orderValue
            });
            bookEditDbAccess.SaveBook(book);
        }

        public void ChangeAuthorLinksOrder(BookAuthorLinksOrderEditedDto authorLinksDto)
        {
            if (authorLinksDto.AuthorLinks == null
                || !authorLinksDto.AuthorLinks.Any())
            {
                AddError(errorMessage: "author links cannot be empty");
                return;
            }
            if (authorLinksDto.AuthorLinks.Select(a => a.Order).Min() < 0)
            {
                AddError(errorMessage: "order values contain negative value");
                return;
            }
            bool hasDuplicates = authorLinksDto.AuthorLinks.Select(a => a.AuthorId)
                .Distinct()
                .Count() != authorLinksDto.AuthorLinks.Count();
            if (hasDuplicates)
            {
                AddError(errorMessage: "order values are duplicated");
                return;
            }
            var book = bookEditDbAccess.GetBookWithAuthorLinks(authorLinksDto.BookId);
            if (book == null)
            {
                AddError(errorMessage: $"book id='{authorLinksDto.BookId}' not found");
                return;
            }
            var authorsLinkDict = book.AuthorsLink
                .ToDictionary(al => al.AuthorId);
            foreach (var authorLink in authorLinksDto.AuthorLinks)
            {
                if (!authorsLinkDict.ContainsKey(authorLink.AuthorId))
                {
                    AddError(errorMessage: $"author id='{authorLink.AuthorId}' not found");
                    return;
                }
                authorsLinkDict[authorLink.AuthorId].Order = authorLink.Order;
            }
            bookEditDbAccess.SaveBook(book);
            return;
        }

        public Guid CreateBook(BookCreateDto newBook)
        {
            return bookCatalogDbAccess.Create(newBook);
        }

        public void RemoveAuthor(BookRemoveAuthorDto removeAuthorDto)
        {
            Book book = bookEditDbAccess.GetBookWithAuthorLinks(bookId: removeAuthorDto.BookId);
            if (book == null)
            {
                AddError(errorMessage: $"book id={removeAuthorDto.BookId} not found");
                return;
            }
            bool hasAuthor = false;
            foreach (var authorLink in book.AuthorsLink)
            {
                if (authorLink.AuthorId == removeAuthorDto.AuthorId)
                {
                    book.AuthorsLink.Remove(authorLink);
                    hasAuthor = true;
                    break;
                }
            }
            if (!hasAuthor)
            {
                AddError(errorMessage: $"author id={removeAuthorDto.AuthorId} not found");
                return;
            }
            bookEditDbAccess.SaveBook(book);
        }
    }
}