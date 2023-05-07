using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Entities;

namespace BookApp.BLL.Services.BookManageImage
{
    public interface IBookManageImagesDbAccess
    {
        Book GetBook(Guid bookId);
        BookImageToEditDto GetBookToEditImage(Guid bookId);
        void SaveBook(Book book);
    }
}
