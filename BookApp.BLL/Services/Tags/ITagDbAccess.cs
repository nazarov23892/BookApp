﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Entities;

namespace BookApp.BLL.Services.Tags
{
    public interface ITagDbAccess
    {
        IEnumerable<TagListItemDto> GetTags();
        int StoreTag(Tag newTag);
    }
}
