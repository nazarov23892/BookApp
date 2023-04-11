using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DataContexts;

namespace ServiceDbAccessLayer.Orders.Concrete
{
    public class PlaceOrderDbAccess : IPlaceOrderDbAccess
    {
        private readonly AppIdentityDbContext efDbContext;

        public PlaceOrderDbAccess(AppIdentityDbContext efDbContext)
        {
            this.efDbContext = efDbContext;
        }

        public void Add(Order newOrder)
        {
            efDbContext.Orders.Add(newOrder);
        }

        public Dictionary<Guid, Book> FindBooksByIds(IEnumerable<Guid> bookIds)
        {
            return efDbContext.Books
                .Where(b => bookIds.Contains(b.BookId))
                .ToDictionary(b => b.BookId);
        }

        public void SaveChanges()
        {
            efDbContext.SaveChanges();
        }
    }
}
