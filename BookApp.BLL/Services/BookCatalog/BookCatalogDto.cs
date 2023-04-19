using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.BookCatalog
{
    public class BookCatalogDto
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<AuthorNameDto> Authors { get; set; }
    }

    public class AuthorNameDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
