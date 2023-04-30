using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Generic;
using BookApp.BLL.Entities;

namespace BookApp.BLL.Services.Tags.Concrete
{
    public class TagService : ServiceErrors, ITagService
    {
        private readonly ITagDbAccess tagDbAccess;

        public TagService(ITagDbAccess tagDbAccess)
        {
            this.tagDbAccess = tagDbAccess;
        }

        public int CreateTag(TagCreateDto newTag)
        {
            Tag tag = new Tag { Text = newTag.Text };
            tagDbAccess.StoreTag(tag);
            return tag.TagId;
        }
    }
}
