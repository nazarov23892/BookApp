using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.Tags
{
    public interface ITagDbAccess
    {
        IEnumerable<TagListItemDto> GetTags();
    }
}
