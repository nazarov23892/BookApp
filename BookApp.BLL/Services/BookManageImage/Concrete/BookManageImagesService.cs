using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BookApp.BLL.Generic;
using BookApp.BLL.Entities;
using BookApp.BLL.Interfaces;

namespace BookApp.BLL.Services.BookManageImage.Concrete
{
    public class BookManageImagesService : ServiceErrors, IBookManageImagesService
    {
        private readonly IBookManageImagesDbAccess bookManageImagesDbAccess;
        private readonly IBookImagesFileAccess bookImagesFileAccess;

        public BookManageImagesService(
            IBookManageImagesDbAccess bookManageImagesDbAccess,
            IBookImagesFileAccess bookImagesFileAccess)
        {
            this.bookManageImagesDbAccess = bookManageImagesDbAccess;
            this.bookImagesFileAccess = bookImagesFileAccess;
        }

        public void SetBookImage(Guid bookId, IFormFileForService file)
        {
            Book book = bookManageImagesDbAccess.GetBook(bookId: bookId);
            if (book == null)
            {
                AddError(errorMessage: $"book not found id='{bookId}'");
                return;
            }
            if (file.Length == 0)
            {
                AddError(errorMessage: $"unable to read file");
                return;
            }
            string extension = Path.GetExtension(file.Filename);
            if (! new[] {".jpeg", ".jpg" }.Contains(extension))
            {
                AddError(errorMessage: $"unsupported file type");
                return;
            }
            if (file.Length > DomainConstants.BookImageMaxSizeBytes)
            {
                AddError(errorMessage: "too large file size");
                return;
            }
            if (!string.IsNullOrEmpty(book.ImageUrl))
            {
                bookImagesFileAccess.RemoveImage(book.ImageUrl);
            }
            string fileName = $"{Guid.NewGuid()}{extension}";
            using (var stream = bookImagesFileAccess.CreateImage(filename: fileName))
            {
                file.CopyTo(stream);
            }
            book.ImageUrl = fileName;
            bookManageImagesDbAccess.SaveBook(book);
        }
    }
}
