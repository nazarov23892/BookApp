using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.BookManage
{
    public class BookTagsForAddDto
    {
        public Guid BookId { get; set; }
        public string BookTitle { get; set; }
        public IEnumerable<BookTagsForAddItemDto> Tags { get; set; }
    }

    public class BookTagsForAddItemDto
    {
        public int TagId { get; set; }
        public string Text { get; set; }
    }
}
