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

            var book1 = Books[0];
            var book2 = Books[1];
            var book3 = Books[2];

            SessionCartService target = new SessionCartService(saver: mock.Object);

            target.Add(book2);
            target.Add(book1);
            target.Add(book3);

            CartLine[] lines = target.Lines
                .ToArray();

            Assert.False(target.HasErrors);

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

            var book1 = Books[0];
            var book2 = Books[1];

            SessionCartService target = new SessionCartService(saver: mock.Object);

            target.Add(book2);
            target.Add(book1);
            target.Add(book1);

            CartLine[] lines = target.Lines
                .ToArray();

            Assert.True(target.HasErrors);
            Assert.Single(target.Errors);
            Assert.Equal("item has already been added", target.Errors.First().ErrorMessage);

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
            var book1 = Books[0];
            var book2 = Books[1];

            SessionCartService target = new SessionCartService(saver: mock.Object);
            target.Add(book1);
            target.Add(book2);

            target.SetQuantity(
                bookId: book1.BookId,
                quantity: 4);
            target.SetQuantity(
                bookId: book2.BookId,
                quantity: 9);

            CartLine[] lines = target.Lines
                .ToArray();

            Assert.False(target.HasErrors);
            Assert.Equal(book1.BookId, lines[0].Book.BookId);
            Assert.Equal(book2.BookId, lines[1].Book.BookId);
            Assert.Equal(4, lines[0].Quantity);
            Assert.Equal(9, lines[1].Quantity);
        }

        [Fact]
        public void Test_SetQuantityWhenNotExist()
        {
            var book1 = Books[0];
            var book2 = Books[1];
            var line1 = new CartLine
            {
                Book = book1,
                Quantity = 2
            };

            Mock<ICartLinesSessionSaver> mock = new Mock<ICartLinesSessionSaver>();
            mock.Setup(m => m.Read()).Returns(() =>
            {
                return new[] { line1 };
            });

            SessionCartService target = new SessionCartService(mock.Object);

            target.SetQuantity(
                bookId: book2.BookId,
                quantity: 4);

            Assert.True(target.HasErrors);
            Assert.Single(target.Errors);
            Assert.Equal("item not found", target.Errors.First().ErrorMessage);
        }

        [Fact]
        public void Test_SetQuantityWhenInvalidQuantity()
        {
            var book1 = Books[0];
            var line1 = new CartLine
            {
                Book = book1,
                Quantity = 2
            };

            Mock<ICartLinesSessionSaver> mock = new Mock<ICartLinesSessionSaver>();
            mock.Setup(m => m.Read()).Returns(() =>
            {
                return new[] { line1 };
            });

            SessionCartService target = new SessionCartService(mock.Object);
  
            target.SetQuantity(
                bookId: book1.BookId,
                quantity: -1);

            Assert.True(target.HasErrors);
            Assert.Single(target.Errors);
            Assert.Equal("cannot be less or equal zero", target.Errors.First().ErrorMessage);
        }

        [Fact]
        public void Test_RestoreState()
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
        public void Test_Remove()
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
        public void Test_RemoveWhenNotExist()
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
        public void Test_Clear()
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
        public void Test_SaveWhenAdd()
        {
            var book1 = Books[0];
            var book2 = Books[1];
            var book3 = Books[2];

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
            
            target.SetQuantity(
                bookId: book2.BookId,
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
    }
}
