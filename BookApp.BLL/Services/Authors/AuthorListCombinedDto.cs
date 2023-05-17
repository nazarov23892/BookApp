using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.Authors
{
    public class AuthorListCombinedDto
    {
        public IEnumerable<AuthorListItemDto> Authors { get; set; }
        public PageOptionsOut PageOptions { get; set; }
    }
}
