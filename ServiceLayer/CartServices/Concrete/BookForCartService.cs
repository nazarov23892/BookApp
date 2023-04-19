using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DataLayer.DataContexts;
//using Microsoft.EntityFrameworkCore;
using ServiceLayer.CartServices.QueryObjects;

namespace ServiceLayer.CartServices.Concrete
{
    public class BookForCartService : IBookForCartService
    {
        //private readonly AppIdentityDbContext efDbContext;

        //public BookForCartService(AppIdentityDbContext efDbContext)
        //{
        //    this.efDbContext = efDbContext;
        //}

        public BookForCartDto GetItem(Guid bookId)
        {
            return null;
                //efDbContext.Books
                //.AsNoTracking()
                //.MapBookToBookForCartDto()
                //.SingleOrDefault(b => b.BookId == bookId);
        }
    }
}
