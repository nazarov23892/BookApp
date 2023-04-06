using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace ServiceLayer.BookServices
{
    public interface IBookCatalogService
    {
        public IEnumerable<Book> GetList();
    }
}
