using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.Authors.Concrete
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorDbAccess authorDbAccess;

        public AuthorService(IAuthorDbAccess authorDbAccess)
        {
            this.authorDbAccess = authorDbAccess;
        }

        public IEnumerable<AuthorListItemDto> GetAuthors()
        {
            return authorDbAccess.GetAuthors();
        }
    }
}
