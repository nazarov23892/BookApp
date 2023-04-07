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
    }
}
