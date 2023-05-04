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
        void AddTag(BookAddTagDto addTagDto);
        void RemoveTag(BookRemoveTagDto removeTagDto);
        void SetDescription(BookDescriptionEditedDto bookDescriptionDto);
    }
}
