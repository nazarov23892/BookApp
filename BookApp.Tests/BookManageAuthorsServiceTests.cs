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
        [Fact]
        public void Cannot_Reorder_Authors_When_AuthorLinks_Are_Empty_Or_Null()
        {
            // Arrange

            Author author1 = new Author { AuthorId = new Guid("00000000-0000-0000-0000-000000000001") };
            Book book1 = new Book
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000051"),
                AuthorsLink = new[] { new BookAuthor { AuthorId = author1.AuthorId, Author = author1, Order = 0} }
                    .ToList()
            };

            Mock<IBookManageAuthorsDbAccess> mock = new Mock<IBookManageAuthorsDbAccess>();
            mock.Setup(m => m.GetBookWithAuthorLinks(It.IsAny<Guid>()))
                .Returns<Guid>(bookId =>
                {
                    return bookId == book1.BookId
                        ? book1
                        : null;
                });

            BookAuthorLinksOrderEditedDto authorLinsOrderedDto1 = new BookAuthorLinksOrderEditedDto
            {
                BookId = book1.BookId,
                AuthorLinks = Array.Empty<BookAuthorLinksOrderEditedItemDto>()
            };
            BookAuthorLinksOrderEditedDto authorLinsOrderedDto2 = new BookAuthorLinksOrderEditedDto
            {
                BookId = book1.BookId,
                AuthorLinks = null
            };

            BookManageAuthorsService target1 = new BookManageAuthorsService(mock.Object);
            BookManageAuthorsService target2 = new BookManageAuthorsService(mock.Object);

            // Act
            target1.ChangeAuthorLinksOrder(authorLinsOrderedDto1);
            target2.ChangeAuthorLinksOrder(authorLinsOrderedDto2);

            // Assert
            Assert.True(target1.HasErrors);
            Assert.True(target2.HasErrors);

            Assert.Single(collection: target1.Errors);
            Assert.Single(collection: target2.Errors);

            Assert.Contains(
                expectedSubstring: "author links cannot be empty",
                actualString: target1.Errors.Single().ErrorMessage);
            Assert.Contains(
                expectedSubstring: "author links cannot be empty",
                actualString: target2.Errors.Single().ErrorMessage);
            mock.Verify(
                expression: m => m.SaveBook(It.IsAny<Book>()),
                times: Times.Never);
        }

        [Fact]
        public void Cannot_Reorder_Authors_When_OrderNums_Have_Negative_Value()
        {
            // Arrange

            Author author1 = new Author { AuthorId = new Guid("00000000-0000-0000-0000-000000000001") };
            Author author2 = new Author { AuthorId = new Guid("00000000-0000-0000-0000-000000000002") };
            Author author3 = new Author { AuthorId = new Guid("00000000-0000-0000-0000-000000000003") };

            Book book1 = new Book
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000051"),
                AuthorsLink = new[]
                {
                    new BookAuthor { AuthorId = author1.AuthorId, Author = author1, Order = 0},
                    new BookAuthor { AuthorId = author2.AuthorId, Author = author2, Order = 1},
                    new BookAuthor { AuthorId = author3.AuthorId, Author = author3, Order = 2},
                }.ToList()
            };

            Mock<IBookManageAuthorsDbAccess> mock = new Mock<IBookManageAuthorsDbAccess>();
            mock.Setup(m => m.GetBookWithAuthorLinks(It.IsAny<Guid>()))
                .Returns<Guid>(bookId =>
                {
                    return bookId == book1.BookId
                        ? book1
                        : null;
                });
            
            BookAuthorLinksOrderEditedDto authorLinsOrderedDto = new BookAuthorLinksOrderEditedDto
            {
                BookId = book1.BookId,
                AuthorLinks = new[]
                {
                    new BookAuthorLinksOrderEditedItemDto { Order = 0, AuthorId = author1.AuthorId },
                    new BookAuthorLinksOrderEditedItemDto { Order = 1, AuthorId = author2.AuthorId },
                    new BookAuthorLinksOrderEditedItemDto { Order = -1, AuthorId = author3.AuthorId },
                }
            };
            BookManageAuthorsService target = new BookManageAuthorsService(mock.Object);

            // Act
            target.ChangeAuthorLinksOrder(authorLinsOrderedDto);

            // Assert
            Assert.True(target.HasErrors);
            Assert.Single(collection: target.Errors);
            Assert.Contains(
                expectedSubstring: "order values contain negative value",
                actualString: target.Errors.Single().ErrorMessage);
            mock.Verify(
                expression: m => m.SaveBook(It.IsAny<Book>()),
                times: Times.Never);
        }

        [Fact]
        public void Cannot_Reorder_Authors_When_OrderNums_Have_Duplicates()
        {
            // Arrange

            Author author1 = new Author { AuthorId = new Guid("00000000-0000-0000-0000-000000000001") };
            Author author2 = new Author { AuthorId = new Guid("00000000-0000-0000-0000-000000000002") };

            Book book1 = new Book
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000051"),
                AuthorsLink = new[]
                {
                    new BookAuthor { AuthorId = author1.AuthorId, Author = author1, Order = 0},
                    new BookAuthor { AuthorId = author2.AuthorId, Author = author2, Order = 1}
                }.ToList()
            };

            Mock<IBookManageAuthorsDbAccess> mock = new Mock<IBookManageAuthorsDbAccess>();
            mock.Setup(m => m.GetBookWithAuthorLinks(It.IsAny<Guid>()))
                .Returns<Guid>(bookId =>
                {
                    return bookId == book1.BookId
                        ? book1
                        : null;
                });

            BookAuthorLinksOrderEditedDto authorLinsOrderedDto = new BookAuthorLinksOrderEditedDto
            {
                BookId = book1.BookId,
                AuthorLinks = new[]
                {
                    new BookAuthorLinksOrderEditedItemDto { Order = 0, AuthorId = author1.AuthorId },
                    new BookAuthorLinksOrderEditedItemDto { Order = 0, AuthorId = author2.AuthorId }
                }
            };
            BookManageAuthorsService target = new BookManageAuthorsService(mock.Object);

            // Act
            target.ChangeAuthorLinksOrder(authorLinsOrderedDto);

            // Assert
            Assert.True(target.HasErrors);
            Assert.Single(collection: target.Errors);
            Assert.Contains(
                expectedSubstring: "order values are duplicated",
                actualString: target.Errors.Single().ErrorMessage);
            mock.Verify(
                expression: m => m.SaveBook(It.IsAny<Book>()),
                times: Times.Never);
        }

        [Fact]
        public void Cannot_Reorder_Authors_When_Authors_Have_Duplicates()
        {
            // Arrange

            Author author1 = new Author { AuthorId = new Guid("00000000-0000-0000-0000-000000000001") };
            Author author2 = new Author { AuthorId = new Guid("00000000-0000-0000-0000-000000000002") };

            Book book1 = new Book
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000051"),
                AuthorsLink = new[]
                {
                    new BookAuthor { AuthorId = author1.AuthorId, Author = author1, Order = 0},
                    new BookAuthor { AuthorId = author2.AuthorId, Author = author2, Order = 1}
                }.ToList()
            };

            Mock<IBookManageAuthorsDbAccess> mock = new Mock<IBookManageAuthorsDbAccess>();
            mock.Setup(m => m.GetBookWithAuthorLinks(It.IsAny<Guid>()))
                .Returns<Guid>(bookId =>
                {
                    return bookId == book1.BookId
                        ? book1
                        : null;
                });

            BookAuthorLinksOrderEditedDto authorLinsOrderedDto = new BookAuthorLinksOrderEditedDto
            {
                BookId = book1.BookId,
                AuthorLinks = new[]
                {
                    new BookAuthorLinksOrderEditedItemDto { Order = 0, AuthorId = author1.AuthorId },
                    new BookAuthorLinksOrderEditedItemDto { Order = 1, AuthorId = author1.AuthorId }
                }
            };
            BookManageAuthorsService target = new BookManageAuthorsService(mock.Object);

            // Act
            target.ChangeAuthorLinksOrder(authorLinsOrderedDto);

            // Assert
            Assert.True(target.HasErrors);
            Assert.Single(collection: target.Errors);
            Assert.Contains(
                expectedSubstring: "authors are duplicated",
                actualString: target.Errors.Single().ErrorMessage);
            mock.Verify(
                expression: m => m.SaveBook(It.IsAny<Book>()),
                times: Times.Never);
        }

        [Fact]
        public void Cannot_Reorder_Authors_When_Book_Is_Not_Exist_In_Db()
        {
            // Arrange

            Author author1 = new Author { AuthorId = new Guid("00000000-0000-0000-0000-000000000001") };
            Author author2 = new Author { AuthorId = new Guid("00000000-0000-0000-0000-000000000002") };

            Book book1 = new Book
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000051"),
                AuthorsLink = new[]
                {
                    new BookAuthor { AuthorId = author1.AuthorId, Author = author1, Order = 0},
                    new BookAuthor { AuthorId = author2.AuthorId, Author = author2, Order = 1}
                }.ToList()
            };

            Mock<IBookManageAuthorsDbAccess> mock = new Mock<IBookManageAuthorsDbAccess>();
            mock.Setup(m => m.GetBookWithAuthorLinks(It.IsAny<Guid>()))
                .Returns<Guid>(bookId =>
                {
                    return bookId == book1.BookId
                        ? book1
                        : null;
                });

            BookAuthorLinksOrderEditedDto authorLinsOrderedDto = new BookAuthorLinksOrderEditedDto
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000052"),
                AuthorLinks = new[]
                {
                    new BookAuthorLinksOrderEditedItemDto { Order = 1, AuthorId = author1.AuthorId },
                    new BookAuthorLinksOrderEditedItemDto { Order = 0, AuthorId = author2.AuthorId }
                }
            };
            BookManageAuthorsService target = new BookManageAuthorsService(mock.Object);

            // Act
            target.ChangeAuthorLinksOrder(authorLinsOrderedDto);

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
        public void Cannot_Reorder_Authors_When_Book_Not_Contains_Author()
        {
            // Arrange

            Author author1 = new Author { AuthorId = new Guid("00000000-0000-0000-0000-000000000001") };
            Author author2 = new Author { AuthorId = new Guid("00000000-0000-0000-0000-000000000002") };
            Author author3 = new Author { AuthorId = new Guid("00000000-0000-0000-0000-000000000003") };

            Book book1 = new Book
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000051"),
                AuthorsLink = new[]
                {
                    new BookAuthor { AuthorId = author1.AuthorId, Author = author1, Order = 0},
                    new BookAuthor { AuthorId = author2.AuthorId, Author = author2, Order = 1}
                }.ToList()
            };

            Mock<IBookManageAuthorsDbAccess> mock = new Mock<IBookManageAuthorsDbAccess>();
            mock.Setup(m => m.GetBookWithAuthorLinks(It.IsAny<Guid>()))
                .Returns<Guid>(bookId =>
                {
                    return bookId == book1.BookId
                        ? book1
                        : null;
                });

            BookAuthorLinksOrderEditedDto authorLinsOrderedDto = new BookAuthorLinksOrderEditedDto
            {
                BookId = book1.BookId,
                AuthorLinks = new[]
                {
                    new BookAuthorLinksOrderEditedItemDto { Order = 0, AuthorId = author1.AuthorId },
                    new BookAuthorLinksOrderEditedItemDto { Order = 1, AuthorId = author2.AuthorId },
                    new BookAuthorLinksOrderEditedItemDto { Order = 2, AuthorId = author3.AuthorId }
                }
            };
            BookManageAuthorsService target = new BookManageAuthorsService(mock.Object);

            // Act
            target.ChangeAuthorLinksOrder(authorLinsOrderedDto);

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

    }
}
