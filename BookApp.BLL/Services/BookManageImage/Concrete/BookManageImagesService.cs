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
        private readonly IEnvironment environment;

        public BookManageImagesService(
            IBookManageImagesDbAccess bookManageImagesDbAccess,
            IEnvironment environment)
        {
            this.bookManageImagesDbAccess = bookManageImagesDbAccess;
            this.environment = environment;
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
            string mediaPath = environment.RootPath;
            string fileName = $"{Guid.NewGuid()}{extension}";
            string fullPath = Path.Combine(mediaPath, @"uploads\images", fileName);
            using (var stream = new FileStream(path: fullPath, mode: FileMode.Create))
            {
                file.CopyTo(stream);
            }
            book.ImageUrl = fileName;
            bookManageImagesDbAccess.SaveBook(book);
        }
    }
}
