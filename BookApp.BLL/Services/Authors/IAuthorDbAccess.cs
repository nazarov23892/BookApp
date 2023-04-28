using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.Authors
{
    public interface IAuthorDbAccess
    {
        IEnumerable<AuthorListItemDto> GetAuthors(); 
    }
}
