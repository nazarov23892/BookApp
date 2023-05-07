using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Generic;

namespace BookApp.BLL.Services.BookManageImage
{
    public interface IBookManageImagesService: IServiceErrors
    {
        void SetBookImage(Guid bookId, string imageFilename);
    }
}
