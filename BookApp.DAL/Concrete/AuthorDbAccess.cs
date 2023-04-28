using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookApp.DAL.DataContexts;
using BookApp.BLL.Services.Authors;

namespace BookApp.DAL.Concrete
{
    public class AuthorDbAccess : IAuthorDbAccess
    {
        private readonly AppIdentityDbContext efDbContext;

        public AuthorDbAccess(AppIdentityDbContext efDbContext)
        {
            this.efDbContext = efDbContext;
        }

        public IEnumerable<AuthorListItemDto> GetAuthors()
        {
            var authors = efDbContext.Authors
                .AsNoTracking()
                .Select(a => new AuthorListItemDto
                {
                    AuthorId = a.AuthorId,
                    Firstname = a.Firstname,
                    Lastname = a.Lastname
                })
                .ToArray();
            return authors;
        }
    }
}
