using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Orders;
using BookApp.Shared.DTOs.Orders;
using Domain.Entities;

namespace DataLayer.Concrete
{
    public class PlaceOrderDbAccess : IPlaceOrderDbAccess
    {
        public void Add(Order newOrder)
        {
            throw new NotImplementedException();
        }

        public Dictionary<Guid, Book> FindBooksByIds(IEnumerable<Guid> bookIds)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
