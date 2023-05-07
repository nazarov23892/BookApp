using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.BookManageImage
{
    public class BookImageToEditDto
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
    }
}
