using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Generic;

namespace BookApp.BLL.Services.Tags
{
    public interface ITagService: IServiceErrors
    {
        int CreateTag(TagCreateDto newTag);
    }
}
