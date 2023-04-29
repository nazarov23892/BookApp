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

        public BookEditAuthorsCombinedDto GetBookForEditAuthors(Guid bookId)
        {
            IEnumerable<BookEditAuthorsItemAuthorDto> authors = bookEditDbAccess.GetAuthors();
            BookEditAuthorsDto book = bookEditDbAccess.GetBookForEditAuthors(bookId);
            return new BookEditAuthorsCombinedDto
            {
                Book = book,
                Authors = authors
            };
        }


    }
}