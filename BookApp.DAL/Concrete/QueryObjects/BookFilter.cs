using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Entities;

namespace BookApp.DAL.Concrete.QueryObjects
{
    public static class BookFilter
    {
        public static IQueryable<Book> FilterByTag(
            this IQueryable<Book> query,
            int tagId)
        {
            return query.Where(b => tagId == 0
                || b.Tags.Where(t => t.TagId == tagId).Any());
        }
    }
}
