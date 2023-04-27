using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.BookCatalog
{
    public class BookEditAuthorsCombinedDto
    {
        public BookEditAuthorsDto Book { get; set; }
        public IEnumerable<BookEditAuthorsItemAuthorDto> Authors { get; set; }
    }
}
