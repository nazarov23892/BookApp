﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookApp.DAL.DataContexts;
using BookApp.BLL.Services.Tags;
using BookApp.BLL.Entities;

namespace BookApp.DAL.Concrete
{
    public class TagDbAccess : ITagDbAccess
    {
        private readonly AppIdentityDbContext efDbContext;

        public TagDbAccess(AppIdentityDbContext efDbContext)
        {
            this.efDbContext = efDbContext;
        }

        public IEnumerable<TagListItemDto> GetTags()
        {
            return efDbContext.Set<Tag>()
                .AsNoTracking()
                .Select(t => new TagListItemDto
                {
                    TagId = t.TagId,
                    Text = t.Text
                })
                .ToArray();
        }
    }
}
