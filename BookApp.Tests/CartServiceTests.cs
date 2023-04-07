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
        public void Test_Add()
        {
            Mock<ICartLinesSessionSaver> mock = new Mock<ICartLinesSessionSaver>();
            mock.Setup(m => m.Read()).Returns(() =>
                {
                    return Enumerable.Empty<CartLine>();
                });

            var book1 = new BookForCartDto
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000001"),
                Title = "book-1",
                Price = 1.1M
            };
            var book2 = new BookForCartDto
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000002"),
                Title = "book-2",
                Price = 2.2M
            };
            var book3 = new BookForCartDto
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000003"),
                Title = "book-3",
                Price = 3.3M
            };

            SessionCartService target = new SessionCartService(saver: mock.Object);

            target.Add(book2);
            target.Add(book1);
            target.Add(book3);

            CartLine[] lines = target.Lines
                .ToArray();

            Assert.Equal(3, lines.Length);
            Assert.Equal(book2.BookId, lines[0].Book.BookId);
            Assert.Equal(book1.BookId, lines[1].Book.BookId);
            Assert.Equal(book3.BookId, lines[2].Book.BookId);

            Assert.Equal(1, lines[0].Quantity);
            Assert.Equal(1, lines[1].Quantity);
            Assert.Equal(1, lines[2].Quantity);
        }

        [Fact]
        public void Test_AddWhenExist()
        {
            Mock<ICartLinesSessionSaver> mock = new Mock<ICartLinesSessionSaver>();
            mock.Setup(m => m.Read()).Returns(() =>
            {
                return Enumerable.Empty<CartLine>();
            });

            var book1 = new BookForCartDto
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000001"),
                Title = "book-1",
                Price = 1.1M
            };
            var book2 = new BookForCartDto
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000002"),
                Title = "book-2",
                Price = 2.2M
            };

            SessionCartService target = new SessionCartService(saver: mock.Object);

            target.Add(book2);
            target.Add(book1);
            target.Add(book1);

            CartLine[] lines = target.Lines
                .ToArray();

            Assert.Equal(2, lines.Length);
            Assert.Equal(book2.BookId, lines[0].Book.BookId);
            Assert.Equal(book1.BookId, lines[1].Book.BookId);
            Assert.Equal(1, lines[0].Quantity);
            Assert.Equal(1, lines[1].Quantity);
        }

        [Fact]
        public void Test_SetQuantity()
        {
            Mock<ICartLinesSessionSaver> mock = new Mock<ICartLinesSessionSaver>();
            mock.Setup(m => m.Read()).Returns(() =>
            {
                return Enumerable.Empty<CartLine>();
            });
            var book1 = new BookForCartDto
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000001"),
                Title = "book-1",
                Price = 1.1M
            };
            var book2 = new BookForCartDto
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000002"),
                Title = "book-2",
                Price = 2.2M
            };

            SessionCartService target = new SessionCartService(saver: mock.Object);
            target.Add(book1);
            target.Add(book2);

            target.SetQuantity(
                book: book1,
                quantity: 4);
            target.SetQuantity(
                book: book2,
                quantity: 9);

            CartLine[] lines = target.Lines
                .ToArray();

            Assert.Equal(book1.BookId, lines[0].Book.BookId);
            Assert.Equal(book2.BookId, lines[1].Book.BookId);
            Assert.Equal(4, lines[0].Quantity);
            Assert.Equal(9, lines[1].Quantity);
        }

        [Fact]
        public void Test_RestoreState()
        {
            var book1 = new BookForCartDto
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000001"),
                Title = "book-1",
                Price = 1.1M
            };
            var book2 = new BookForCartDto
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000002"),
                Title = "book-2",
                Price = 2.2M
            };

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

            Assert.Equal(2, lines.Length);
            Assert.Equal(line2.Book.BookId, lines[0].Book.BookId);
            Assert.Equal(line1.Book.BookId, lines[1].Book.BookId);
            Assert.Equal(line2.Quantity, lines[0].Quantity);
            Assert.Equal(line1.Quantity, lines[1].Quantity);
        }

        [Fact]
        public void Test_Remove()
        {
            var book1 = new BookForCartDto
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000001"),
                Title = "book-1",
                Price = 1.1M
            };
            var book2 = new BookForCartDto
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000002"),
                Title = "book-2",
                Price = 2.2M
            };
            var book3 = new BookForCartDto
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000003"),
                Title = "book-3",
                Price = 3.3M
            };
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

            target.Remove(book2);
            CartLine[] linesAfterRemoveBook2 = target.Lines
                .ToArray();

            target.Remove(book3);
            CartLine[] linesAfterRemoveBook3 = target.Lines
                .ToArray();

            target.Remove(book1);
            CartLine[] linesAfterRemoveBook1 = target.Lines
                .ToArray();

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
        public void Test_Clear()
        {
            var line1 = new CartLine
            {
                Book = new BookForCartDto
                {
                    BookId = new Guid("00000000-0000-0000-0000-000000000001"),
                    Title = "book-1",
                    Price = 1.1M
                },
                Quantity = 1
            };
            var line2 = new CartLine
            {
                Book = new BookForCartDto
                {
                    BookId = new Guid("00000000-0000-0000-0000-000000000002"),
                    Title = "book-2",
                    Price = 2.2M
                },
                Quantity = 2
            };
            Mock<ICartLinesSessionSaver> mock = new Mock<ICartLinesSessionSaver>();
            mock.Setup(m => m.Read()).Returns(() =>
            {
                return new[] { line1, line2 };
            });

            SessionCartService target = new SessionCartService(saver: mock.Object);

            target.Clear();

            Assert.False(target.Lines.Any());
        }

        [Fact]
        public void Test_SaveWhenAdd()
        {
            var book1 = new BookForCartDto
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000001"),
                Title = "book-1",
                Price = 1.1M
            };
            var book2 = new BookForCartDto
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000002"),
                Title = "book-2",
                Price = 2.2M
            };
            var book3 = new BookForCartDto
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000003"),
                Title = "book-3",
                Price = 3.3M
            };
            var line1 = new CartLine
            {
                Book = book1,
                Quantity = 5
            };

            CartLine[] tmpLines = null;
            Mock<ICartLinesSessionSaver> mock = new Mock<ICartLinesSessionSaver>();
            mock.Setup(m => m.Read())
                .Returns(
                () =>
                {
                    return new[] { line1 };
                });
            mock.Setup(m => m.Write(It.IsAny<IEnumerable<CartLine>>()))
                .Callback<IEnumerable<CartLine>>(x =>
                {
                    tmpLines = x.ToArray();
                });

            SessionCartService target = new SessionCartService(saver: mock.Object);

            // actions

            target.Add(book3);
            CartLine[] linesAfterAddBook3 = CopyArray(tmpLines);

            target.Add(book2);
            CartLine[] linesAfterAddBook2 = CopyArray(tmpLines);

            // asserts

            Assert.Equal(2, linesAfterAddBook3.Length);
            Assert.Equal(book1.BookId, linesAfterAddBook3[0].Book.BookId);
            Assert.Equal(book3.BookId, linesAfterAddBook3[1].Book.BookId);

            Assert.Equal(3, linesAfterAddBook2.Length);
            Assert.Equal(book1.BookId, linesAfterAddBook2[0].Book.BookId);
            Assert.Equal(book3.BookId, linesAfterAddBook2[1].Book.BookId);
            Assert.Equal(book2.BookId, linesAfterAddBook2[2].Book.BookId);

            mock.Verify(x => x.Write(
                It.IsAny<IEnumerable<CartLine>>()), 
                Times.Exactly(2));
        }

        [Fact]
        public void Test_SaveWhenSetQuantity()
        {
            var book1 = new BookForCartDto
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000001"),
                Title = "book-1",
                Price = 1.1M
            };
            var book2 = new BookForCartDto
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000002"),
                Title = "book-2",
                Price = 2.2M
            };

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
            
            target.SetQuantity(
                book: book2,
                quantity: 8);

            CartLine[] linesAfterSetQuantity = CopyArray(tmpLines);

            // asserts

            Assert.Equal(2, linesAfterSetQuantity.Length);
            Assert.Equal(book1.BookId, linesAfterSetQuantity[0].Book.BookId);
            Assert.Equal(book2.BookId, linesAfterSetQuantity[1].Book.BookId);
            Assert.Equal(5, linesAfterSetQuantity[0].Quantity);
            Assert.Equal(8, linesAfterSetQuantity[1].Quantity);

            mock.Verify(x => x.Write(
                It.IsAny<IEnumerable<CartLine>>()),
                Times.Exactly(1));
        }

        [Fact]
        public void Test_SaveWhenRemove()
        {
            var book1 = new BookForCartDto
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000001"),
                Title = "book-1",
                Price = 1.1M
            };
            var book2 = new BookForCartDto
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000002"),
                Title = "book-2",
                Price = 2.2M
            };

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

            target.Remove(book1);

            CartLine[] linesAfterRemove = CopyArray(tmpLines);

            // asserts

            Assert.Single(linesAfterRemove);
            Assert.Equal(book2.BookId, linesAfterRemove[0].Book.BookId);

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
    }
}
