using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookApp.BLL.Services.BookCatalog
{
    public class PageOptionsIn
    {
        public const int DefaultPageSize = 5;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = DefaultPageSize;
        public int FilterTag { get; set; }
        public PageSortOptions SortOption { get; set; }
    }

    public class PageOptionsOut : PageOptionsIn
    {
        public int PageCount { get; set; }
    }

    public enum PageSortOptions
    {
        [Display(Name = "sort by...")] SimpleOrder = 0,
        [Display(Name = "Price ↓")] ByPriceDesc,
        [Display(Name = "Price ↑")] ByPriceAsc
    }
}
