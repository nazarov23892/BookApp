using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.BookCatalogServices
{
    public class BookDetailsDto
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<AuthorNameDto> Authors { get; set; }
    }
}
