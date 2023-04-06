using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.BookCatalogServices
{
    public class PageOptionsIn
    {
        public const int DefaultPageSize = 10;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = DefaultPageSize;
    }

    public class PageOptionsOut: PageOptionsIn
    {
        public int PageCount { get; set; }
    }

}
