using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace ServiceDbAccessLayer.Orders
{
    public interface IPlaceOrderDbAccess
    {
        Dictionary<Guid, Book> FindBooksByIds(IEnumerable<Guid> bookIds);
        void Add(Order newOrder);
        void SaveChanges();
    }
}
