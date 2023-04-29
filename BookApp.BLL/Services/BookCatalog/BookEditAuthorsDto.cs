using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.BookCatalog
{
    public class BookEditAuthorsDto
    {
        public Guid BookId { get; set; }
        public string BookTitle { get; set; }
        public IEnumerable<BookEditAuthorsItemAuthorDto> ChosenAuthorsIds { get; set; }
    }

    public class BookEditAuthorsItemAuthorDto
    {
        public Guid AuthorId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int OrderNo { get; set; }
    }

}
