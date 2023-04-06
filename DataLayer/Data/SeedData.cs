using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DataContexts;
using DataLayer.Entities;

namespace DataLayer.Data
{
    public static class SeedData
    {
        public static void RunSeed(AppIdentityDbContext efDbContext)
        {
            const int numBooks = 64;

            SeedBooks(
                efDbContext: efDbContext,
                books: GetBooks(num: numBooks));
        }

        private static IEnumerable<Book> GetBooks(int num)
        {
            List<Book> books = new List<Book>();

            decimal itemPrice = 10.0M;
            for (int i = 0; i < num; i++)
            {
                books.Add(new Book
                {
                    BookId = default(Guid),
                    Title = $"book-title={1 + i}",
                    Price = itemPrice
                });
                itemPrice += (i / 10M);
            }
            return books;
        }

        private static void SeedBooks(
            AppIdentityDbContext efDbContext,
            IEnumerable<Book> books)
        {
            if (efDbContext.Books.Any())
            {
                return;
            }
            efDbContext.AddRange(books);
            efDbContext.SaveChanges();
        }
    }
}
