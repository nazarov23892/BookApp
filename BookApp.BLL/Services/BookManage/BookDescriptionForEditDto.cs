using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.BookManage
{
    public class BookDescriptionForEditDto
    {
        public Guid BookId { get; set; }
        public string BookTitle { get; set; }
        public string Description { get; set; }
    }
}
