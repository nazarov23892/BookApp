using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace ServiceLayer.BookCatalogServices
{
    public class BookListCombinedDto
    {
        public PageOptionsOut PageOptionsOut { get; set; }
        public IEnumerable<Book> Books { get; set; }
    }
}
