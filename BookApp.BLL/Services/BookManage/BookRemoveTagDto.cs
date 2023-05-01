using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.BookManage
{
    public class BookRemoveTagDto
    {
        public Guid BookId { get; set; }
        public int TagId { get; set; }
    }
}
