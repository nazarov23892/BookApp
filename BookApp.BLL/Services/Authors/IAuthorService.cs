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
        AuthorListCombinedDto GetAuthors(PageOptionsIn pageOptions);

        Guid CreateAuthor(AuthorCreateDto newAuthor);
    }
}
