using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.BookCatalog
{
    public class BookListCombinedDto
    {
        public PageOptionsOut PageOptionsOut { get; set; }
        public IEnumerable<BookCatalogDto> Books { get; set; }
    }
}
