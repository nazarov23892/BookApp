﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookApp.BLL.Services.Cart;
using DataLayer.DataContexts;
using DataLayer.Concrete.QueryObjects;

namespace DataLayer.Concrete
{
    public class BookForCartDbAccess : IBookForCartDbAccess
    {
        private readonly AppIdentityDbContext efDbContext;

        public BookForCartDbAccess(AppIdentityDbContext efDbContext)
        {
            this.efDbContext = efDbContext;
        }

        public BookForCartDto GetItem(Guid bookId)
        {
            return efDbContext.Books
            .AsNoTracking()
            .MapBookToBookForCartDto()
            .SingleOrDefault(b => b.BookId == bookId);
        }
    }
}
