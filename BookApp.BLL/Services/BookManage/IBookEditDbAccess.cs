﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Entities;

namespace BookApp.BLL.Services.BookManage
{
    public interface IBookEditDbAccess
    {
        Guid Create(BookCreateDto newBook);
        void SaveBook(Book book);
        BookWithTagsDto GetBookForEditTags(Guid bookId);
        BookTagsForAddDto GetTagsForAdd(Guid bookId);
        Book GetBookWithTags(Guid bookId);
        Tag GetTag(int tagId);
        BookDescriptionForEditDto GetBookForEditDescription(Guid bookId);
        Book GetBook(Guid bookId);
    }
}
