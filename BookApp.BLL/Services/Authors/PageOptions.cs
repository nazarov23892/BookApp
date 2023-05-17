using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.Authors
{
    public class PageOptions
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
