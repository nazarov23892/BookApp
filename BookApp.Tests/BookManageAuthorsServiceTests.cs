using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using BookApp.BLL.Services.BookManageAuthors;
using BookApp.BLL.Services.BookManageAuthors.Concrete;
using BookApp.BLL.Entities;

namespace BookApp.Tests
{
    public class BookManageAuthorsServiceTests
    {

        [Fact]
        public void Cannot_Add_Author_When_Book_Is_Not_Exist_In_Db()
        {
            // Arrange
            Mock<IBookManageAuthorsDbAccess> mock = new Mock<IBookManageAuthorsDbAccess>();
            mock.Setup(m => m.GetAuthor(It.IsAny<Guid>()))
                .Returns<Guid>(authorId =>
                {
                    var author = new Author
                    {
                        AuthorId = new Guid("00000000-0000-0000-0000-000000000001")
                    };
                    return authorId == author.AuthorId
                        ? author
                        : null;
                });
            mock.Setup(m => m.GetBookWithAuthorLinks(It.IsAny<Guid>()))
                .Returns<Guid>(bookId =>
                {
                    var book = new Book
                    {
                        BookId = new Guid("00000000-0000-0000-0000-000000000051"),
                        AuthorsLink = Array.Empty<BookAuthor>()
                    };
                    return bookId == book.BookId
                        ? book
                        : null;
                });
            BookManageAuthorsService target = new BookManageAuthorsService(mock.Object);

            // Act
            target.AddAuthor(
                addAuthorDto: new BookAddAuthorDto
                {
                    AuthorId = new Guid("00000000-0000-0000-0000-000000000001"),
                    BookId = new Guid("00000000-0000-0000-0000-000000000052")
                });

            // Assert
            Assert.True(target.HasErrors);
            Assert.Single(collection: target.Errors);
            Assert.Contains(
                expectedSubstring: "book not found",
                actualString: target.Errors.Single().ErrorMessage);
            mock.Verify(
                expression: m => m.SaveBook(It.IsAny<Book>()),
                times: Times.Never);
        }

        [Fact]
        public void Cannot_Add_Author_When_Author_Is_Not_Exist_In_Db()
        {
            // Arrange
            Mock<IBookManageAuthorsDbAccess> mock = new Mock<IBookManageAuthorsDbAccess>();
            mock.Setup(m => m.GetAuthor(It.IsAny<Guid>()))
                .Returns<Guid>(authorId =>
                {
                    var author = new Author
                    {
                        AuthorId = new Guid("00000000-0000-0000-0000-000000000001")
                    };
                    return authorId == author.AuthorId
                        ? author
                        : null;
                });
            mock.Setup(m => m.GetBookWithAuthorLinks(It.IsAny<Guid>()))
                .Returns<Guid>(bookId =>
                {
                    var book = new Book
                    {
                        BookId = new Guid("00000000-0000-0000-0000-000000000051"),
                        AuthorsLink = Array.Empty<BookAuthor>()
                    };
                    return bookId == book.BookId
                        ? book
                        : null;
                });
            BookManageAuthorsService target = new BookManageAuthorsService(mock.Object);

            // Act
            target.AddAuthor(
                addAuthorDto: new BookAddAuthorDto
                {
                    AuthorId = new Guid("00000000-0000-0000-0000-000000000002"),
                    BookId = new Guid("00000000-0000-0000-0000-000000000051")
                });

            // Assert
            Assert.True(target.HasErrors);
            Assert.Single(collection: target.Errors);
            Assert.Contains(
                expectedSubstring: "author not found",
                actualString: target.Errors.Single().ErrorMessage);
            mock.Verify(
                expression: m => m.SaveBook(It.IsAny<Book>()),
                times: Times.Never);
        }

        [Fact]
        public void Cannot_Add_Author_When_Author_Is_Already_Added()
        {
            // Arrange
            Mock<IBookManageAuthorsDbAccess> mock = new Mock<IBookManageAuthorsDbAccess>();
            mock.Setup(m => m.GetAuthor(It.IsAny<Guid>()))
                .Returns<Guid>(authorId =>
                {
                    var author = new Author
                    {
                        AuthorId = new Guid("00000000-0000-0000-0000-000000000001")
                    };
                    return authorId == author.AuthorId
                        ? author
                        : null;
                });
            mock.Setup(m => m.GetBookWithAuthorLinks(It.IsAny<Guid>()))
                .Returns<Guid>(bookId =>
                {
                    var book = new Book
                    {
                        BookId = new Guid("00000000-0000-0000-0000-000000000051"),
                        AuthorsLink = new[]
                        {
                            new BookAuthor
                            {
                                AuthorId = new Guid("00000000-0000-0000-0000-000000000001")
                            }
                        }.ToList()
                    };
                    return bookId == book.BookId
                        ? book
                        : null;
                });
            BookManageAuthorsService target = new BookManageAuthorsService(mock.Object);

            // Act
            target.AddAuthor(
                addAuthorDto: new BookAddAuthorDto
                {
                    AuthorId = new Guid("00000000-0000-0000-0000-000000000001"),
                    BookId = new Guid("00000000-0000-0000-0000-000000000051")
                });

            // Assert
            Assert.True(target.HasErrors);
            Assert.Single(collection: target.Errors);
            Assert.Contains(
                expectedSubstring: "book already contains given author",
                actualString: target.Errors.Single().ErrorMessage);
            mock.Verify(
                expression: m => m.SaveBook(It.IsAny<Book>()),
                times: Times.Never);
        }

        [Fact]
        public void Can_Add_Author()
        {
            // Arrange
            Book bookInDb = new Book
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000051"),
                AuthorsLink = Array.Empty<BookAuthor>().ToList()
            };
            Author authorInDb = new Author { AuthorId = new Guid("00000000-0000-0000-0000-000000000001") };

            Mock<IBookManageAuthorsDbAccess> mock = new Mock<IBookManageAuthorsDbAccess>();
            mock.Setup(m => m.GetAuthor(It.IsAny<Guid>()))
                .Returns<Guid>(authorId =>
                {
                    return authorId == authorInDb.AuthorId
                        ? authorInDb
                        : null;
                });
            mock.Setup(m => m.GetBookWithAuthorLinks(It.IsAny<Guid>()))
                .Returns<Guid>(bookId =>
                {
                    return bookId == bookInDb.BookId
                        ? bookInDb
                        : null;
                });
            BookManageAuthorsService target = new BookManageAuthorsService(mock.Object);

            // Act
            target.AddAuthor(
                addAuthorDto: new BookAddAuthorDto
                {
                    AuthorId = new Guid("00000000-0000-0000-0000-000000000001"),
                    BookId = new Guid("00000000-0000-0000-0000-000000000051")
                });

            // Assert
            Assert.False(target.HasErrors);
            mock.Verify(
                expression: m => m.SaveBook(It.IsAny<Book>()),
                times: Times.Once);
            mock.Verify(m => m.SaveBook(It.Is<Book>(b =>
                b == bookInDb
                && b.AuthorsLink != null
                && b.AuthorsLink.Count == 1
                && b.AuthorsLink.Single().Author == authorInDb
                )));
        }
    }
}
