using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Generic;

namespace BookApp.BLL.Services.Authors
{
    public interface IAuthorService: IServiceErrors
    {
        IEnumerable<AuthorListItemDto> GetAuthors();

        Guid CreateAuthor(AuthorCreateDto newAuthor);
    }
}
