using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookApp.DAL.DataContexts;
using BookApp.BLL.Services.BookManageImage;
using BookApp.BLL.Entities;

namespace BookApp.DAL.Concrete
{
    public class BookManageImagesDbAccess : IBookManageImagesDbAccess
    {
        private readonly AppIdentityDbContext efDbContext;

        public BookManageImagesDbAccess(AppIdentityDbContext efDbContext)
        {
            this.efDbContext = efDbContext;
        }

        public Book GetBook(Guid bookId)
        {
            return efDbContext.Books
                .SingleOrDefault(b => b.BookId == bookId);
        }

        public BookImageToEditDto GetBookToEditImage(Guid bookId)
        {
            return efDbContext.Books
                .AsNoTracking()
                .Select(b => new BookImageToEditDto
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    ImageUrl = b.ImageUrl
                })
                .SingleOrDefault(b => b.BookId == bookId);
        }

        public void SaveBook(Book book)
        {
            efDbContext.Books.Update(book);
            efDbContext.SaveChanges();
        }
    }
}
