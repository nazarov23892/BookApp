﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.BookManageAuthors
{
    public class BookAddAuthorDto
    {
        public Guid BookId { get; set; }
        public Guid AuthorId { get; set; }
    }
}
