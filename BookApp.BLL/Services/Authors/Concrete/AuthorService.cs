using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Generic;
using BookApp.BLL.Entities;

namespace BookApp.BLL.Services.Authors.Concrete
{
    public class AuthorService : ServiceErrors, IAuthorService
    {
        private readonly IAuthorDbAccess authorDbAccess;

        public AuthorService(IAuthorDbAccess authorDbAccess)
        {
            this.authorDbAccess = authorDbAccess;
        }

        public Guid CreateAuthor(AuthorCreateDto newAuthor)
        {
            Author author = new Author
            {
                AuthorId = Guid.NewGuid(),
                Firstname = newAuthor.Firstname,
                Lastname = newAuthor.Lastname
            };
            authorDbAccess.StoreAuthor(author);
            return author.AuthorId;
        }

        public IEnumerable<AuthorListItemDto> GetAuthors(PageOptions pageOptions)
        {
            return authorDbAccess.GetAuthors(
                pageStartsZero: pageOptions.Page - 1,
                pageSize: pageOptions.PageSize);
        }
    }
}
