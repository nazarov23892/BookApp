using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.Authors
{
    public class PageOptionsIn
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }

    public class PageOptionsOut: PageOptionsIn
    {
        public int PageCount { get; set; }
    }
}