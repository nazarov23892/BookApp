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
        BookImageToEditDto GetBookToEditImage(Guid bookId);
    }
}
