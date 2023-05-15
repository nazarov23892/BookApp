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
        private int _pageSize = DefaultPageSize;
        private int[] _pageSizes = new[] { 5, 10, 20, 50 };

        public const int DefaultPageSize = 5;
        public int Page { get; set; } = 1;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = _pageSizes.Contains(value)
                ? value
                : DefaultPageSize;
        }
        public int FilterTag { get; set; }
        public PageSortOptions SortOption { get; set; }
        public IEnumerable<int> PageSizes
        {
            get => _pageSizes;
        }
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
