using System;
using Xunit;
using ServiceLayer.CartServices;
using ServiceLayer.CartServices.Concrete;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Moq;

namespace BookApp.Tests
{
    public class CartServiceTests
    {
        [Fact]
        public void Can_Add()
        {
            // Arrange

            BookForCartDto book1 = null, book2 = null;
            {
                var arr = GenerateBookDtoArray(2);
                book1 = arr[0];
                book2 = arr[1];
            }

            Mock<ICartLinesSessionSaver> mock = new Mock<ICartLinesSessionSaver>();
            CartLine[] savedLines = null;
            mock.Setup(m => m.Read()).Returns(() =>
                {
                    return Enumerable.Empty<CartLine>();
                });
            mock.Setup(m => m.Write(It.IsAny<IEnumerable<CartLine>>()))
                .Callback<IEnumerable<CartLine>>(x =>
                {
                    savedLines = x.ToArray();
                });
            SessionCartService target = new SessionCartService(saver: mock.Object);

            // Act

            target.Add(book2);
            var state_1 = CopyArray(sourceArr: savedLines);
            target.Add(book1);
            var state_2 = CopyArray(sourceArr: savedLines);

            // Assert

            BookForCartDto baselineBook1 = null, baselineBook2 = null;
            {
                var arr = GenerateBookDtoArray(2);
                baselineBook1 = arr[0];
                baselineBook2 = arr[1];
            }

            CartLine[] lines = target.Lines
                .ToArray();

            Assert.False(target.HasErrors);

            Assert.Equal(expected: 2, actual: lines.Length);
            Assert.Equal(1, lines[0].Quantity);
            Assert.Equal(1, lines[1].Quantity);
            Assert.Equal(expected: baselineBook2.BookId, actual: lines[0].Book.BookId);
            Assert.Equal(expected: baselineBook1.BookId, actual: lines[1].Book.BookId);

            Assert.Single(state_1);
            Assert.Equal(expected: baselineBook2.BookId, actual: state_1[0].Book.BookId);

            Assert.Equal(expected: 2, state_2.Length);
            Assert.Equal(expected: baselineBook2.BookId, actual: state_2[0].Book.BookId);
            Assert.Equal(expected: baselineBook1.BookId, actual: state_2[1].Book.BookId);

            mock.Verify(m => m.Write(It.IsAny<IEnumerable<CartLine>>()), Times.Exactly(2));
        }

        [Fact]
        public void Cannot_Add_When_Exist()
        {
            // Arrange

            BookForCartDto book1 = null, book2 = null;
            {
                var arr = GenerateBookDtoArray(2);
                book1 = arr[0];
                book2 = arr[1];
            }

            Mock<ICartLinesSessionSaver> mock = new Mock<ICartLinesSessionSaver>();
            mock.Setup(m => m.Read()).Returns(() =>
            {
                return new[] { book2, book1 }
                    .Select(b => new CartLine
                    {
                        Book = b,
                        Quantity = 1
                    });
            });
            SessionCartService target = new SessionCartService(saver: mock.Object);

            // Act

            target.Add(book2);

            // Assert

            BookForCartDto baselineBook1 = null, baselineBook2 = null;
            {
                var arr = GenerateBookDtoArray(2);
                baselineBook1 = arr[0];
                baselineBook2 = arr[1];
            }
            CartLine[] lines = target.Lines
                .ToArray();

            Assert.True(target.HasErrors);
            Assert.Single(target.Errors);
            Assert.Equal("item has already been added", target.Errors.First().ErrorMessage);

            Assert.Equal(2, lines.Length);
            Assert.Equal(expected: baselineBook2.BookId, actual: lines[0].Book.BookId);
            Assert.Equal(expected: baselineBook1.BookId, actual: lines[1].Book.BookId);
            Assert.Equal(expected: 1, actual: lines[0].Quantity);
            Assert.Equal(expected: 1, actual: lines[1].Quantity);

            mock.Verify(m => m.Write(It.IsAny<IEnumerable<CartLine>>()), Times.Never);
        }

        [Fact]
        public void Cannot_Add_When_Lines_Limit_Reached()
        {
            // Arrange

            BookForCartDto[] books = GenerateBookDtoArray(11);
            Mock<ICartLinesSessionSaver> mock = new Mock<ICartLinesSessionSaver>();
            mock.Setup(m => m.Read()).Returns(() =>
            {
                return books
                    .Select(b => new CartLine
                    {
                        Book = b,
                        Quantity = 1
                    })
                    .Take(10);
            });
            SessionCartService target = new SessionCartService(saver: mock.Object);

            // Act

            var bookExtra = books[10];
            target.Add(bookExtra);

            // Assert

            CartLine[] lines = target.Lines
                .ToArray();

            Assert.Equal(expected: 10, actual: lines.Length);
            Assert.True(target.HasErrors);
            Assert.Single(target.Errors);
            Assert.Equal(
                expected: "limit of line items reached",
                actual: target.Errors.First().ErrorMessage);

            mock.Verify(x => x.Write(It.IsAny<IEnumerable<CartLine>>()),
                Times.Never);
        }

        [Fact]
        public void Can_SetQuantity()
        {
            // Arrange

            BookForCartDto book1 = null, book2 = null;
            {
                var arr = GenerateBookDtoArray(2);
                book1 = arr[0];
                book2 = arr[1];
            }
            Mock<ICartLinesSessionSaver> mock = new Mock<ICartLinesSessionSaver>();
            mock.Setup(m => m.Read()).Returns(() =>
            {
                return new[] { book2, book1 }
                    .Select(b => new CartLine
                    {
                        Book = b,
                        Quantity = 4
                    });
            });
            CartLine[] savedLines = null;
            mock.Setup(m => m.Write(It.IsAny<IEnumerable<CartLine>>()))
                .Callback<IEnumerable<CartLine>>(x =>
                {
                    savedLines = x.ToArray();
                });
            SessionCartService target = new SessionCartService(saver: mock.Object);

            // Act

            target.SetQuantity(
                bookId: book2.BookId,
                quantity: 9);

            // Assert

            BookForCartDto baselineBook1 = null, baselineBook2 = null;
            {
                var arr = GenerateBookDtoArray(2);
                baselineBook1 = arr[0];
                baselineBook2 = arr[1];
            }

            CartLine[] lines = target.Lines
                .ToArray();

            Assert.False(target.HasErrors);
            Assert.Equal(expected: baselineBook2.BookId, actual: lines[0].Book.BookId);
            Assert.Equal(expected: baselineBook1.BookId, actual: lines[1].Book.BookId);
            Assert.Equal(expected: 9, lines[0].Quantity);
            Assert.Equal(expected: 4, lines[1].Quantity);

            mock.Verify(m => m.Write(It.IsAny<IEnumerable<CartLine>>()), times: Times.Once);
            Assert.Equal(expected: 2, savedLines.Length);
            Assert.Equal(expected: baselineBook2.BookId, actual: savedLines[0].Book.BookId);
            Assert.Equal(expected: baselineBook1.BookId, actual: savedLines[1].Book.BookId);
        }

        [Fact]
        public void Cannot_SetQuantity_When_Not_Exist()
        {
            // Arrange

            BookForCartDto book1 = null, book2 = null;
            {
                var arr = GenerateBookDtoArray(2);
                book1 = arr[0];
                book2 = arr[1];
            }
            Mock<ICartLinesSessionSaver> mock = new Mock<ICartLinesSessionSaver>();
            mock.Setup(m => m.Read()).Returns(() =>
            {
                return new[] { book1 }
                    .Select(b => new CartLine { Book = b, Quantity = 1 });
            });
            SessionCartService target = new SessionCartService(mock.Object);

            // Act

            target.SetQuantity(
                bookId: book2.BookId,
                quantity: 4);

            // Assert

            Assert.True(target.HasErrors);
            Assert.Single(target.Errors);
            Assert.Equal("item not found", target.Errors.First().ErrorMessage);
            mock.Verify(m => m.Write(It.IsAny<IEnumerable<CartLine>>()), times: Times.Never);
        }

        [Fact]
        public void Cannot_SetQuantity_When_Value_Equal_Or_Less_Than_Zero()
        {
            // Arrange

            BookForCartDto book1 = null;
            {
                var arr = GenerateBookDtoArray(2);
                book1 = arr[0];
            }
            Mock<ICartLinesSessionSaver> mock = new Mock<ICartLinesSessionSaver>();
            mock.Setup(m => m.Read()).Returns(() =>
            {
                return new[] { new CartLine { Book = book1, Quantity = 1 } };
            });
            SessionCartService target1 = new SessionCartService(mock.Object);
            SessionCartService target2 = new SessionCartService(mock.Object);

            // Act

            target1.SetQuantity(
                bookId: book1.BookId,
                quantity: -1);
            target2.SetQuantity(
                bookId: book1.BookId,
                quantity: 0);

            // Assert

            Assert.True(target1.HasErrors);
            Assert.True(target2.HasErrors);

            Assert.Single(target1.Errors);
            Assert.Single(target2.Errors);

            Assert.Equal("cannot be less or equal zero", target1.Errors.First().ErrorMessage);
            Assert.Equal("cannot be less or equal zero", target2.Errors.First().ErrorMessage);

            mock.Verify(m => m.Write(It.IsAny<IEnumerable<CartLine>>()), times: Times.Never);
        }

        [Fact]
        public void Can_Restore_State()
        {
            var book1 = Books[0];
            var book2 = Books[1];

            var line1 = new CartLine
            {
                Book = book1,
                Quantity = 2
            };
            var line2 = new CartLine
            {
                Book = book2,
                Quantity = 3
            };

            Mock<ICartLinesSessionSaver> mock = new Mock<ICartLinesSessionSaver>();
            mock.Setup(m => m.Read()).Returns(() =>
            {
                return new[] { line2, line1 };
            });

            SessionCartService target = new SessionCartService(saver: mock.Object);
            CartLine[] lines = target.Lines
                .ToArray();

            Assert.False(target.HasErrors);
            Assert.Equal(2, lines.Length);
            Assert.Equal(line2.Book.BookId, lines[0].Book.BookId);
            Assert.Equal(line1.Book.BookId, lines[1].Book.BookId);
            Assert.Equal(line2.Quantity, lines[0].Quantity);
            Assert.Equal(line1.Quantity, lines[1].Quantity);
        }

        [Fact]
        public void Can_Remove()
        {
            var book1 = Books[0];
            var book2 = Books[1];
            var book3 = Books[2];

            Mock<ICartLinesSessionSaver> mock = new Mock<ICartLinesSessionSaver>();
            mock.Setup(m => m.Read()).Returns(() =>
            {
                return new[]
                {
                    new CartLine { Book = book1, Quantity = 11 },
                    new CartLine { Book = book2, Quantity = 12 },
                    new CartLine { Book = book3, Quantity = 13 }
                };
            });

            SessionCartService target = new SessionCartService(mock.Object);

            target.Remove(book2.BookId);
            CartLine[] linesAfterRemoveBook2 = target.Lines
                .ToArray();

            target.Remove(book3.BookId);
            CartLine[] linesAfterRemoveBook3 = target.Lines
                .ToArray();

            target.Remove(book1.BookId);
            CartLine[] linesAfterRemoveBook1 = target.Lines
                .ToArray();

            Assert.False(target.HasErrors);

            Assert.Equal(2, linesAfterRemoveBook2.Length);
            Assert.Equal(book1.BookId, linesAfterRemoveBook2[0].Book.BookId);
            Assert.Equal(book3.BookId, linesAfterRemoveBook2[1].Book.BookId);
            Assert.Equal(11, linesAfterRemoveBook2[0].Quantity);
            Assert.Equal(13, linesAfterRemoveBook2[1].Quantity);

            Assert.Single(linesAfterRemoveBook3);
            Assert.Equal(book1.BookId, linesAfterRemoveBook2[0].Book.BookId);
            Assert.Equal(11, linesAfterRemoveBook2[0].Quantity);

            Assert.False(linesAfterRemoveBook1.Any());
        }

        [Fact]
        public void Cannot_Remove_When_Not_Exist()
        {
            var book1 = Books[0];
            var book2 = Books[1];
            var book3 = Books[2];

            Mock<ICartLinesSessionSaver> mock = new Mock<ICartLinesSessionSaver>();
            mock.Setup(m => m.Read()).Returns(() =>
            {
                return new[]
                {
                    new CartLine { Book = book1, Quantity = 11 },
                    new CartLine { Book = book2, Quantity = 12 },
                };
            });

            SessionCartService target = new SessionCartService(mock.Object);

            target.Remove(book3.BookId);
            CartLine[] linesAfterRemoveBook3 = target.Lines
                .ToArray();

            Assert.True(target.HasErrors);
            Assert.Single(target.Errors);
            Assert.Equal("item not found", target.Errors.First().ErrorMessage);

            Assert.Equal(2, linesAfterRemoveBook3.Length);
            Assert.Equal(book1.BookId, linesAfterRemoveBook3[0].Book.BookId);
            Assert.Equal(book2.BookId, linesAfterRemoveBook3[1].Book.BookId);
            Assert.Equal(11, linesAfterRemoveBook3[0].Quantity);
            Assert.Equal(12, linesAfterRemoveBook3[1].Quantity);
        }

        [Fact]
        public void Can_Clear()
        {
            var line1 = new CartLine
            {
                Book = Books[0],
                Quantity = 1
            };
            var line2 = new CartLine
            {
                Book = Books[1],
                Quantity = 2
            };
            Mock<ICartLinesSessionSaver> mock = new Mock<ICartLinesSessionSaver>();
            mock.Setup(m => m.Read()).Returns(() =>
            {
                return new[] { line1, line2 };
            });

            SessionCartService target = new SessionCartService(saver: mock.Object);

            target.Clear();

            Assert.False(target.HasErrors);
            Assert.False(target.Lines.Any());
        }

        [Fact]
        public void Test_SaveWhenRemove()
        {
            var book1 = Books[0];
            var book2 = Books[1];

            CartLine[] tmpLines = null;
            Mock<ICartLinesSessionSaver> mock = new Mock<ICartLinesSessionSaver>();
            mock.Setup(m => m.Read())
                .Returns(
                () =>
                {
                    return new[]
                    {
                        new CartLine { Book = book1, Quantity = 5 },
                        new CartLine { Book = book2, Quantity = 10 }
                    };
                });
            mock.Setup(m => m.Write(It.IsAny<IEnumerable<CartLine>>()))
                .Callback<IEnumerable<CartLine>>(x =>
                {
                    tmpLines = x.ToArray();
                });

            SessionCartService target = new SessionCartService(saver: mock.Object);

            // actions

            target.Remove(book1.BookId);

            CartLine[] linesAfterRemove = CopyArray(tmpLines);

            // asserts

            Assert.Single(linesAfterRemove);
            Assert.Equal(book2.BookId, linesAfterRemove[0].Book.BookId);

            mock.Verify(x => x.Write(
                It.IsAny<IEnumerable<CartLine>>()),
                Times.Exactly(1));
        }

        [Fact]
        public void Test_SaveWhenClear()
        {
            var book1 = Books[0];
            var book2 = Books[1];

            CartLine[] tmpLines = null;
            Mock<ICartLinesSessionSaver> mock = new Mock<ICartLinesSessionSaver>();
            mock.Setup(m => m.Read())
                .Returns(
                () =>
                {
                    return new[]
                    {
                        new CartLine { Book = book1, Quantity = 5 },
                        new CartLine { Book = book2, Quantity = 10 }
                    };
                });
            mock.Setup(m => m.Write(It.IsAny<IEnumerable<CartLine>>()))
                .Callback<IEnumerable<CartLine>>(x =>
                {
                    tmpLines = x.ToArray();
                });

            SessionCartService target = new SessionCartService(saver: mock.Object);

            // actions

            target.Clear();

            // asserts

            Assert.False(tmpLines.Any());
            mock.Verify(x => x.Write(
                It.IsAny<IEnumerable<CartLine>>()),
                Times.Exactly(1));
        }

        private CartLine[] CopyArray(CartLine[] sourceArr)
        {
            CartLine[] res = new CartLine[sourceArr.Length];
            for (int i = 0; i < sourceArr.Length; i++)
            {
                res[i] = new CartLine
                {
                    Book = sourceArr[i].Book,
                    Quantity = sourceArr[i].Quantity
                };
            }
            return res;
        }

        public BookForCartDto[] Books
        {
            get => new[]
            {
                new BookForCartDto
                {
                    BookId = new Guid("00000000-0000-0000-0000-000000000001"),
                    Title = "book-1",
                    Price = 1.1M
                },
                new BookForCartDto
                {
                    BookId = new Guid("00000000-0000-0000-0000-000000000002"),
                    Title = "book-2",
                    Price = 2.2M
                },
                new BookForCartDto
                {
                    BookId = new Guid("00000000-0000-0000-0000-000000000003"),
                    Title = "book-3",
                    Price = 3.3M
                }
            };
        }

        private BookForCartDto[] GenerateBookDtoArray(int num)
        {
            List<BookForCartDto> list = new List<BookForCartDto>();
            for (int i = 0; i < num; i++)
            {
                string guidString = String.Format("{0:00000000-0000-0000-0000-000000000000}", 1 + i);
                list.Add(new BookForCartDto
                {
                    BookId = new Guid(guidString),
                    Title = $"book-{1 + i}",
                    Price = 10M,
                });
            }
            return list.ToArray();
        }
    }
}
