using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Entities;

namespace BookApp.BLL.Services.Authors
{
    public interface IAuthorDbAccess
    {
        IEnumerable<AuthorListItemDto> GetAuthors(int pageStartsZero, int pageSize);
        void StoreAuthor(Author newAuthor);
    }
}
