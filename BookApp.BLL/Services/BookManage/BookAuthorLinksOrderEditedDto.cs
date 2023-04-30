using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.BookManage
{
    public class BookAuthorLinksOrderEditedDto
    {
        public Guid BookId { get; set; }
        public IEnumerable<BookAuthorLinksOrderEditedItemDto> AuthorLinks { get; set; }
    }

    public class BookAuthorLinksOrderEditedItemDto
    {
        public Guid AuthorId { get; set; }
        public int Order { get; set; }
    }
}
