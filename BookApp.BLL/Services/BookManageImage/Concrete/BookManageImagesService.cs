using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Generic;
using BookApp.BLL.Entities;
using System.IO;

namespace BookApp.BLL.Services.BookManageImage.Concrete
{
    public class BookManageImagesService : ServiceErrors, IBookManageImagesService
    {
        private readonly IBookManageImagesDbAccess bookManageImagesDbAccess;

        public BookManageImagesService(IBookManageImagesDbAccess bookManageImagesDbAccess)
        {
            this.bookManageImagesDbAccess = bookManageImagesDbAccess;
        }

        public void SetBookImage(Guid bookId, string imageFilename)
        {
            Book book = bookManageImagesDbAccess.GetBook(bookId: bookId);
            if (book == null)
            {
                AddError(errorMessage: $"book not found id='{bookId}'");
                return;
            }
            book.ImageUrl = imageFilename;
            bookManageImagesDbAccess.SaveBook(book);
        }
    }
}
