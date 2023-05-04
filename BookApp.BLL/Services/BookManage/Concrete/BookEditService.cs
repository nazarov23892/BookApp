using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Generic;
using BookApp.BLL.Entities;
using BookApp.BLL.Services.BookCatalog;

namespace BookApp.BLL.Services.BookManage.Concrete
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

        public void AddTag(BookAddTagDto addTagDto)
        {
            var book = bookEditDbAccess.GetBookWithTags(bookId: addTagDto.BookId);

            // check book exist in db
            if (book == null)
            {
                AddError(errorMessage: $"book id={addTagDto.BookId} not found");
                return;
            }

            var tag = bookEditDbAccess.GetTag(tagId: addTagDto.TagId);

            // check tags exist in db
            if (tag == null)
            {
                AddError(errorMessage: $"tag id={addTagDto.TagId} not found");
                return;
            }

            // check tag already exist in book
            bool alreadyExist = book.Tags.
                Where(t => t.TagId == tag.TagId)
                .Any();
            if (alreadyExist)
            {
                AddError(errorMessage: "book already contains given tag");
                return;
            }
            book.Tags.Add(tag);
            bookEditDbAccess.SaveBook(book);
            return;
        }

        public Guid CreateBook(BookCreateDto newBook)
        {
            return bookEditDbAccess.Create(newBook);
        }

        public void RemoveTag(BookRemoveTagDto removeTagDto)
        {
            Book book = bookEditDbAccess.GetBookWithTags(bookId: removeTagDto.BookId);
            if (book == null)
            {
                AddError(errorMessage: $"book id={removeTagDto.BookId} not found");
                return;
            }
            var tag = book.Tags
                .SingleOrDefault(t=>t.TagId == removeTagDto.TagId);
            if (tag == null)
            {
                AddError(errorMessage: $"tag id={removeTagDto.TagId} not found");
                return;
            }
            book.Tags.Remove(tag);
            bookEditDbAccess.SaveBook(book);
            return;
        }

        public void SetDescription(BookDescriptionEditedDto bookDescriptionDto)
        {
            PerformValidationObjectProperties(bookDescriptionDto.Description);
            if (HasErrors)
            {
                return;
            }
            Book book = bookEditDbAccess.GetBook(bookId: bookDescriptionDto.BookId);
            if (book == null)
            {
                AddError(errorMessage: $"book not found id='{bookDescriptionDto.BookId}'");
                return;
            }
            book.Description = bookDescriptionDto.Description;
            bookEditDbAccess.SaveBook(book);
        }
    }
}
