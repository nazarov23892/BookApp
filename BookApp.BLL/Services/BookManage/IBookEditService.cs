using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Generic;

namespace BookApp.BLL.Services.BookManage
{
    public interface IBookEditService : IServiceErrors
    {
        Guid CreateBook(BookCreateDto newBook);
        void ChangeAuthorLinksOrder(BookAuthorLinksOrderEditedDto authorLinksDto);
        void AddAuthor(BookAddAuthorDto addAuthorDto);
        void RemoveAuthor(BookRemoveAuthorDto removeAuthorDto);
    }
}
