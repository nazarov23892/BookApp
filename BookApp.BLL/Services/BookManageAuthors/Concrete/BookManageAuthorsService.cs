using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Generic;
using BookApp.BLL.Entities;

namespace BookApp.BLL.Services.BookManageAuthors.Concrete
{
    public class BookManageAuthorsService : ServiceErrors, IBookManageAuthorsService
    {
        private readonly IBookManageAuthorsDbAccess bookManageAuthorDbAccess;

        public BookManageAuthorsService(IBookManageAuthorsDbAccess bookManageAuthorDbAccess)
        {
            this.bookManageAuthorDbAccess = bookManageAuthorDbAccess;
        }

        public void AddAuthor(BookAddAuthorDto addAuthorDto)
        {
            // take book with authors
            // check book exist in db
            var book = bookManageAuthorDbAccess.GetBookWithAuthorLinks(bookId: addAuthorDto.BookId);
            if (book == null)
            {
                AddError(errorMessage: $"book not found id={addAuthorDto.BookId}");
                return;
            }
            BookAuthor[] bookAuthorsArray = book.AuthorsLink
                .ToArray();

            // check author exist in db
            var author = bookManageAuthorDbAccess.GetAuthor(authorId: addAuthorDto.AuthorId);
            if (author == null)
            {
                AddError(errorMessage: $"author not found id={addAuthorDto.AuthorId}");
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
            bookManageAuthorDbAccess.SaveBook(book);
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
            var book = bookManageAuthorDbAccess.GetBookWithAuthorLinks(authorLinksDto.BookId);
            if (book == null)
            {
                AddError(errorMessage: $"book not found id='{authorLinksDto.BookId}'");
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
            bookManageAuthorDbAccess.SaveBook(book);
            return;
        }

        public void RemoveAuthor(BookRemoveAuthorDto removeAuthorDto)
        {
            Book book = bookManageAuthorDbAccess.GetBookWithAuthorLinks(bookId: removeAuthorDto.BookId);
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
            bookManageAuthorDbAccess.SaveBook(book);
        }
    }
}
