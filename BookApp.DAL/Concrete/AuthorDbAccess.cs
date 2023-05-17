using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookApp.DAL.DataContexts;
using BookApp.BLL.Services.Authors;
using BookApp.BLL.Entities;

namespace BookApp.DAL.Concrete
{
    public class AuthorDbAccess : IAuthorDbAccess
    {
        private readonly AppIdentityDbContext efDbContext;

        public AuthorDbAccess(AppIdentityDbContext efDbContext)
        {
            this.efDbContext = efDbContext;
        }

        public IEnumerable<AuthorListItemDto> GetAuthors(int pageStartsZero, int pageSize)
        {
            if (pageStartsZero < 0)
            {
                throw new ArgumentException(
                    message: "cannot be less than zero", 
                    paramName: nameof(pageStartsZero));
            }
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    message: "cannot be less or equal zero",
                    paramName: nameof(pageSize));
            }
            var authors = efDbContext.Authors
                .AsNoTracking()
                .Select(a => new AuthorListItemDto
                {
                    AuthorId = a.AuthorId,
                    Firstname = a.Firstname,
                    Lastname = a.Lastname
                })
                .Skip(count: pageStartsZero * pageSize)
                .Take(count: pageSize)
                .ToArray();
            return authors;
        }

        public void StoreAuthor(Author newAuthor)
        {
            efDbContext.Authors.Add(newAuthor);
            efDbContext.SaveChanges();
        }
    }
}
