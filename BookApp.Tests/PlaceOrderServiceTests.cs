using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using ServiceLayer.OrderServices;
using ServiceLayer.OrderServices.Concrete;
using ServiceDbAccessLayer.Orders;
using Domain.Entities;
using ServiceLayer.Abstract;

namespace BookApp.Tests
{
    public class PlaceOrderServiceTests
    {
        [Fact]
        public void Cannot_PlaceOrder_When_Empty_or_Null_Lines()
        {
            // Arrange
            Mock<IPlaceOrderDbAccess> dbAccessMock = new Mock<IPlaceOrderDbAccess>();
            Mock<ISignInContext> signInContextMock = new Mock<ISignInContext>();

            PlaceOrderService target1 = new PlaceOrderService(
                placeOrderDbAccess: dbAccessMock.Object,
                signInContext: signInContextMock.Object);
            PlaceOrderService target2 = new PlaceOrderService(
                placeOrderDbAccess: dbAccessMock.Object,
                signInContext: signInContextMock.Object);

            // Act
            target1.PlaceOrder(placeOrderDataIn: new PlaceOrderDto
            {
                Firstname = "firstname",
                Lastname = "lastname",
                PhoneNumber = "111",
                Lines = Enumerable.Empty<PlaceOrderLineItemDto>()
            });
            target2.PlaceOrder(placeOrderDataIn: new PlaceOrderDto
            {
                Firstname = "firstname",
                Lastname = "lastname",
                PhoneNumber = "111",
                Lines = null
            });


            // Assert
            Assert.True(target1.HasErrors);
            Assert.True(target2.HasErrors);

            Assert.Single(target1.Errors);
            Assert.Single(target2.Errors);

            var error1 = target1.Errors.First();
            var error2 = target2.Errors.First();

            Assert.True(error1.ErrorMessage.Contains(
                value: "No items in your order.",
                comparisonType: StringComparison.OrdinalIgnoreCase));
            Assert.True(error2.ErrorMessage.Contains(
                value: "No items in your order.",
                comparisonType: StringComparison.OrdinalIgnoreCase));

            dbAccessMock.Verify(x => x.SaveChanges(),
                Times.Never);
            dbAccessMock.Verify(x => x.Add(It.IsAny<Order>()),
                Times.Never);
        }

        [Fact]
        public void Cannot_PlaceOrder_When_Firstname_Empty()
        {
            // Arrange
            Mock<IPlaceOrderDbAccess> dbAccessMock = new Mock<IPlaceOrderDbAccess>();
            Mock<ISignInContext> signInContextMock = new Mock<ISignInContext>();

            PlaceOrderService target1 = new PlaceOrderService(
                placeOrderDbAccess: dbAccessMock.Object,
                signInContext: signInContextMock.Object);

            // Act
            target1.PlaceOrder(placeOrderDataIn: new PlaceOrderDto
            {
                Firstname = string.Empty,
                Lastname = "lastname",
                PhoneNumber = "111",
                Lines = new[]
                {
                    new PlaceOrderLineItemDto
                    {
                        BookId = new Guid("00000000-0000-0000-0000-000000000001"),
                        Quantity = 1,
                        Price = 10.1M
                    },
                    new PlaceOrderLineItemDto
                    {
                        BookId = new Guid("00000000-0000-0000-0000-000000000002"),
                        Quantity = 2,
                        Price = 10.2M
                    }
                }
            });


            // Assert
            Assert.True(target1.HasErrors);
            Assert.Single(target1.Errors);
            var error1 = target1.Errors.First();
            Assert.True(error1.ErrorMessage
                .Contains(value: "Firstname field is required", comparisonType: StringComparison.OrdinalIgnoreCase));
            dbAccessMock.Verify(x => x.Add(It.IsAny<Order>()),
                Times.Never);
            dbAccessMock.Verify(x => x.SaveChanges(),
                Times.Never);
        }

        [Fact]
        public void Cannot_PlaceOrder_When_Lastname_Empty()
        {
            // Arrange
            Mock<IPlaceOrderDbAccess> dbAccessMock = new Mock<IPlaceOrderDbAccess>();
            Mock<ISignInContext> signInContextMock = new Mock<ISignInContext>();

            PlaceOrderService target1 = new PlaceOrderService(
                placeOrderDbAccess: dbAccessMock.Object,
                signInContext: signInContextMock.Object);

            // Act
            target1.PlaceOrder(placeOrderDataIn: new PlaceOrderDto
            {
                Firstname = "firstname",
                Lastname = string.Empty,
                PhoneNumber = "111",
                Lines = new[]
                {
                    new PlaceOrderLineItemDto
                    {
                        BookId = new Guid("00000000-0000-0000-0000-000000000001"),
                        Quantity = 1,
                        Price = 10.1M
                    }
                }
            });

            // Assert
            Assert.True(target1.HasErrors);
            Assert.Single(target1.Errors);
            var error1 = target1.Errors.First();
            Assert.True(error1.ErrorMessage
                .Contains(value: "Lastname field is required", comparisonType: StringComparison.OrdinalIgnoreCase));
            dbAccessMock.Verify(x => x.Add(It.IsAny<Order>()),
                Times.Never);
            dbAccessMock.Verify(x => x.SaveChanges(),
                Times.Never);
        }

        [Fact]
        public void Cannot_PlaceOrder_When_Phonenumber_Empty()
        {
            // Arrange
            Mock<IPlaceOrderDbAccess> dbAccessMock = new Mock<IPlaceOrderDbAccess>();
            Mock<ISignInContext> signInContextMock = new Mock<ISignInContext>();

            PlaceOrderService target1 = new PlaceOrderService(
                placeOrderDbAccess: dbAccessMock.Object,
                signInContext: signInContextMock.Object);

            // Act
            target1.PlaceOrder(placeOrderDataIn: new PlaceOrderDto
            {
                Firstname = "firstname",
                Lastname = "lastname",
                PhoneNumber = string.Empty,
                Lines = new[]
                {
                    new PlaceOrderLineItemDto
                    {
                        BookId = new Guid("00000000-0000-0000-0000-000000000001"),
                        Quantity = 1,
                        Price = 10.1M
                    }
                }
            });

            // Assert
            Assert.True(target1.HasErrors);
            Assert.Single(target1.Errors);
            var error1 = target1.Errors.First();
            Assert.True(error1.ErrorMessage
                .Contains(value: "Phonenumber field is required", comparisonType: StringComparison.OrdinalIgnoreCase));
            dbAccessMock.Verify(x => x.Add(It.IsAny<Order>()),
                Times.Never);
            dbAccessMock.Verify(x => x.SaveChanges(),
                Times.Never);
        }

        [Fact]
        public void Cannot_PlaceOrder_When_Quantity_is_less_or_equal_zero()
        {
            var dbBook1 = new Book
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000001"),
                Title = "book-1",
                Price = 10.1M
            };

            // Arrange
            Mock<IPlaceOrderDbAccess> dbAccessMock = new Mock<IPlaceOrderDbAccess>();
            Mock<ISignInContext> signInContextMock = new Mock<ISignInContext>();

            dbAccessMock.Setup(m => m.FindBooksByIds(It.IsAny<IEnumerable<Guid>>()))
               .Returns<IEnumerable<Guid>>((ids) => new[] { dbBook1 }
                        .ToDictionary(b => b.BookId));

            PlaceOrderService target1 = new PlaceOrderService(
                placeOrderDbAccess: dbAccessMock.Object,
                signInContext: signInContextMock.Object);
            PlaceOrderService target2 = new PlaceOrderService(
                placeOrderDbAccess: dbAccessMock.Object,
                signInContext: signInContextMock.Object);

            var dto1 = new PlaceOrderDto
            {
                Firstname = "firstname",
                Lastname = "lastname",
                PhoneNumber = "111",
                Lines = new[]
                {
                    new PlaceOrderLineItemDto
                    {
                        BookId = dbBook1.BookId,
                        Price = dbBook1.Price,
                        Title = dbBook1.Title,
                        Quantity = 0
                    }
                }
            };
            var dto2 = new PlaceOrderDto
            {
                Firstname = "firstname",
                Lastname = "lastname",
                PhoneNumber = "111",
                Lines = new[]
                {
                    new PlaceOrderLineItemDto
                    {
                        BookId = dbBook1.BookId,
                        Price = dbBook1.Price,
                        Title = dbBook1.Title,
                        Quantity = -1
                    }
                }
            };

            // Act
            target1.PlaceOrder(placeOrderDataIn: dto1);
            target2.PlaceOrder(placeOrderDataIn: dto2);

            // Assert
            Assert.True(target1.HasErrors);
            Assert.True(target2.HasErrors);

            Assert.Single(target1.Errors);
            Assert.Single(target2.Errors);

            var error1 = target1.Errors.First();
            var error2 = target2.Errors.First();

            Assert.True(error1.ErrorMessage.Contains(
                value: "must be between",
                comparisonType: StringComparison.OrdinalIgnoreCase));
            Assert.True(error2.ErrorMessage.Contains(
                value: "must be between",
                comparisonType: StringComparison.OrdinalIgnoreCase));

            dbAccessMock.Verify(x => x.Add(It.IsAny<Order>()),
                Times.Never);
            dbAccessMock.Verify(x => x.SaveChanges(),
                Times.Never);
        }

        [Fact]
        public void Cannot_PlaceOrder_When_Quantity_Exceeded()
        {

            // Arrange
            Mock<IPlaceOrderDbAccess> dbAccessMock = new Mock<IPlaceOrderDbAccess>();
            Mock<ISignInContext> signInContextMock = new Mock<ISignInContext>();

            dbAccessMock.Setup(m => m.FindBooksByIds(It.IsAny<IEnumerable<Guid>>()))
               .Returns<IEnumerable<Guid>>((ids) => GenerateBooks(2)
                        .ToDictionary(b => b.BookId));

            PlaceOrderService target1 = new PlaceOrderService(
                placeOrderDbAccess: dbAccessMock.Object,
                signInContext: signInContextMock.Object);

            var quantityExceeded = 1 + Domain.DomainConstants.MaxQuantityToBuy;
            var books = GenerateBooks(2);
            var dto1 = new PlaceOrderDto
            {
                Firstname = "firstname",
                Lastname = "lastname",
                PhoneNumber = "111",
                Lines = new[] 
                {
                    new PlaceOrderLineItemDto{ BookId = books[0].BookId, 
                        Price = books[0].Price, Quantity = 1 },
                    new PlaceOrderLineItemDto{ BookId = books[1].BookId,
                        Price = books[1].Price, Quantity = quantityExceeded }
                }
            };

            // Act
            target1.PlaceOrder(placeOrderDataIn: dto1);

            // Assert
            Assert.True(target1.HasErrors);
            Assert.Single(target1.Errors);
            Assert.Contains(
                expectedSubstring: "must be between",
                actualString: target1.Errors.First().ErrorMessage);

            dbAccessMock.Verify(x => x.Add(It.IsAny<Order>()),
                Times.Never);
            dbAccessMock.Verify(x => x.SaveChanges(),
                Times.Never);
        }

        [Fact]
        public void Cannot_PlaceOrder_When_Chosen_Id_is_missing_in_Db()
        {
            var dbBook1 = new Book
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000001"),
                Title = "book-1",
                Price = 1.1M
            };
            var dbBook2 = new Book
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000002"),
                Title = "book-2",
                Price = 1.1M
            };
            var missingId = new Guid("00000000-0000-0000-0000-000000000003");
            var lines = new[]
            {
                new PlaceOrderLineItemDto 
                {
                    BookId = dbBook1.BookId,
                    Title = dbBook1.Title,
                    Price = dbBook1.Price,
                    Quantity = 1
                },
                new PlaceOrderLineItemDto
                {
                    BookId = dbBook2.BookId,
                    Title = dbBook2.Title,
                    Price = dbBook2.Price,
                    Quantity = 1
                },
                new PlaceOrderLineItemDto
                {
                    BookId = missingId,
                    Title = "title",
                    Price = 1.1M,
                    Quantity = 1
                }
            };

            // Arrange
            Mock<IPlaceOrderDbAccess> dbAccessMock = new Mock<IPlaceOrderDbAccess>();
            Mock<ISignInContext> signInContextMock = new Mock<ISignInContext>();

            dbAccessMock.Setup(m => m.FindBooksByIds(It.IsAny<IEnumerable<Guid>>()))
               .Returns<IEnumerable<Guid>>((ids) => new[] { dbBook1, dbBook2 }
                        .ToDictionary(b=>b.BookId));

            var dto1 = new PlaceOrderDto
            {
                Firstname = "firstname",
                Lastname = "lastname",
                PhoneNumber = "111",
                Lines = lines
            };
            PlaceOrderService target1 = new PlaceOrderService(
                placeOrderDbAccess: dbAccessMock.Object,
                signInContext: signInContextMock.Object);

            // Act

            Action act = () => target1.PlaceOrder(placeOrderDataIn: dto1);
            var exception = Assert.Throws<InvalidOperationException>(act);

            // Assert

            Assert.NotNull(exception);
            Assert.True(exception.Message.Contains(
                value: "A placing of order failed: book id",
                comparisonType: StringComparison.OrdinalIgnoreCase));

            dbAccessMock.Verify(x => x.Add(It.IsAny<Order>()),
                Times.Never);
            dbAccessMock.Verify(x => x.SaveChanges(),
                Times.Never);
        }

        [Fact]
        public void Cannot_PlaceOrder_When_Has_Invalid_Price()
        {
            var dbBook1 = new Book
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000001"),
                Title = "book-1",
                Price = 1.1M
            };
            var dbBook2 = new Book
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000002"),
                Title = "book-2",
                Price = 0M
            };
            var lines = new[]
            {
                new PlaceOrderLineItemDto
                {
                    BookId = dbBook1.BookId,
                    Title = dbBook1.Title,
                    Price = dbBook1.Price,
                    Quantity = 1
                },
                new PlaceOrderLineItemDto
                {
                    BookId = dbBook2.BookId,
                    Title = dbBook2.Title,
                    Price = dbBook2.Price,
                    Quantity = 1
                }
            };

            // Arrange
            Mock<IPlaceOrderDbAccess> dbAccessMock = new Mock<IPlaceOrderDbAccess>();
            Mock<ISignInContext> signInContextMock = new Mock<ISignInContext>();

            dbAccessMock.Setup(m => m.FindBooksByIds(It.IsAny<IEnumerable<Guid>>()))
               .Returns<IEnumerable<Guid>>((ids) => new[] { dbBook1, dbBook2 }
                        .ToDictionary(b => b.BookId));


            var dto1 = new PlaceOrderDto
            {
                Firstname = "firstname",
                Lastname = "lastname",
                PhoneNumber = "111",
                Lines = lines
            };
            PlaceOrderService target1 = new PlaceOrderService(
                placeOrderDbAccess: dbAccessMock.Object,
                signInContext: signInContextMock.Object);

            // Act

            target1.PlaceOrder(placeOrderDataIn: dto1);

            // Assert

            Assert.True(target1.HasErrors);
            Assert.Single(target1.Errors);
            var error = target1.Errors.Single();
            Assert.Equal(expected: "invalid price value", actual: error.ErrorMessage);

            dbAccessMock.Verify(x => x.Add(It.IsAny<Order>()),
                Times.Never);
            dbAccessMock.Verify(x => x.SaveChanges(),
                Times.Never);
        }

        [Fact]
        public void Cannot_PlaceOrder_When_Price_Expired()
        {
            var dbBook1 = new Book
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000001"),
                Title = "book-1",
                Price = 1.1M
            };
            var dbBook2 = new Book
            {
                BookId = new Guid("00000000-0000-0000-0000-000000000002"),
                Title = "book-2",
                Price = 2.2M
            };
            var lines = new[]
            {
                new PlaceOrderLineItemDto
                {
                    BookId = dbBook1.BookId,
                    Title = dbBook1.Title,
                    Price = dbBook1.Price,
                    Quantity = 1
                },
                new PlaceOrderLineItemDto
                {
                    BookId = dbBook2.BookId,
                    Title = dbBook2.Title,
                    Price = 1.1M,
                    Quantity = 1
                }
            };

            // Arrange
            Mock<IPlaceOrderDbAccess> dbAccessMock = new Mock<IPlaceOrderDbAccess>();
            Mock<ISignInContext> signInContextMock = new Mock<ISignInContext>();

            dbAccessMock.Setup(m => m.FindBooksByIds(It.IsAny<IEnumerable<Guid>>()))
               .Returns<IEnumerable<Guid>>((ids) => new[] { dbBook1, dbBook2 }
                        .ToDictionary(b => b.BookId));

            var dto1 = new PlaceOrderDto
            {
                Firstname = "firstname",
                Lastname = "lastname",
                PhoneNumber = "111",
                Lines = lines
            };
            PlaceOrderService target1 = new PlaceOrderService(
                placeOrderDbAccess: dbAccessMock.Object,
                signInContext: signInContextMock.Object);

            // Act

            target1.PlaceOrder(placeOrderDataIn: dto1);

            // Assert

            Assert.True(target1.HasErrors);
            Assert.Single(target1.Errors);
            var error = target1.Errors.Single();
            Assert.Equal(expected: "items have expired price", actual: error.ErrorMessage);

            dbAccessMock.Verify(x => x.Add(It.IsAny<Order>()),
                Times.Never);
            dbAccessMock.Verify(x => x.SaveChanges(),
                Times.Never);
        }

        [Fact]
        public void Cannot_PlaceOrder_When_Too_Many_LineItems()
        {
            var books = GenerateBooks(num: 11); // todo: rewrite
            var lines = books.Select(b => new PlaceOrderLineItemDto
            {
                BookId = b.BookId,
                Title = b.Title,
                Price = b.Price,
                Quantity = 1
            });

            // Arrange
            Mock<IPlaceOrderDbAccess> dbAccessMock = new Mock<IPlaceOrderDbAccess>();
            Mock<ISignInContext> signInContextMock = new Mock<ISignInContext>();

            dbAccessMock.Setup(m => m.FindBooksByIds(It.IsAny<IEnumerable<Guid>>()))
               .Returns<IEnumerable<Guid>>((ids) => books
                        .ToDictionary(b => b.BookId));

            var dto1 = new PlaceOrderDto
            {
                Firstname = "firstname",
                Lastname = "lastname",
                PhoneNumber = "111",
                Lines = lines
            };
            PlaceOrderService target1 = new PlaceOrderService(
                placeOrderDbAccess: dbAccessMock.Object,
                signInContext: signInContextMock.Object);

            // Act

            target1.PlaceOrder(placeOrderDataIn: dto1);

            // Assert

            Assert.True(target1.HasErrors);
            Assert.Single(target1.Errors);
            var error = target1.Errors.Single();

            Assert.Contains(
                expectedSubstring: "order line items limit exceeded",
                actualString: error.ErrorMessage);

            dbAccessMock.Verify(x => x.Add(It.IsAny<Order>()),
                Times.Never);
            dbAccessMock.Verify(x => x.SaveChanges(),
                Times.Never);
        }

        [Fact]
        public void Cannot_PlaceOrder_When_Not_SignedIn()
        {

            // Arrange
            Mock<IPlaceOrderDbAccess> dbAccessMock = new Mock<IPlaceOrderDbAccess>();
            Mock<ISignInContext> signInContextMock = new Mock<ISignInContext>();

            dbAccessMock.Setup(m => m.FindBooksByIds(It.IsAny<IEnumerable<Guid>>()))
               .Returns<IEnumerable<Guid>>((ids) => GenerateBooks(num: 1)
                        .ToDictionary(b => b.BookId));

            signInContextMock.Setup(m => m.IsSignedIn)
                .Returns(() => false);
            signInContextMock.Setup(m => m.UserId)
                .Returns(() => "123456");

            var dto1 = new PlaceOrderDto
            {
                Firstname = "firstname",
                Lastname = "lastname",
                PhoneNumber = "111",
                Lines = GenerateBooks(num: 1)
                    .Select(b => new PlaceOrderLineItemDto
                    {
                        BookId = b.BookId,
                        Price = b.Price,
                        Quantity = 2
                    })
            };
            PlaceOrderService target1 = new PlaceOrderService(
                placeOrderDbAccess: dbAccessMock.Object,
                signInContext: signInContextMock.Object);

            // Act

            target1.PlaceOrder(placeOrderDataIn: dto1);

            // Assert

            Assert.True(target1.HasErrors);
            Assert.Single(target1.Errors);
            Assert.Contains(
                expectedSubstring: "unauthorized users cannot place an order",
                actualString: target1.Errors.Single().ErrorMessage);

            dbAccessMock.Verify(x => x.Add(It.IsAny<Order>()),
                Times.Never);
            dbAccessMock.Verify(x => x.SaveChanges(),
                Times.Never);
        }

        [Fact]
        public void Cannot_PlaceOrder_When_UserId_is_Empty()
        {

            // Arrange
            Mock<IPlaceOrderDbAccess> dbAccessMock = new Mock<IPlaceOrderDbAccess>();
            Mock<ISignInContext> signInContextUserIdEmptyMock = new Mock<ISignInContext>();
            Mock<ISignInContext> signInContextUserIdNullMock = new Mock<ISignInContext>();

            dbAccessMock.Setup(m => m.FindBooksByIds(It.IsAny<IEnumerable<Guid>>()))
               .Returns<IEnumerable<Guid>>((ids) => GenerateBooks(num: 1)
                        .ToDictionary(b => b.BookId));

            signInContextUserIdEmptyMock.Setup(m => m.IsSignedIn)
                .Returns(() => true);
            signInContextUserIdEmptyMock.Setup(m => m.UserId)
                .Returns(() => string.Empty);

            signInContextUserIdNullMock.Setup(m => m.IsSignedIn)
               .Returns(() => true);
            signInContextUserIdNullMock.Setup(m => m.UserId)
                .Returns(() => null);

            var dto1 = new PlaceOrderDto
            {
                Firstname = "firstname",
                Lastname = "lastname",
                PhoneNumber = "111",
                Lines = GenerateBooks(num: 1)
                    .Select(b => new PlaceOrderLineItemDto
                    {
                        BookId = b.BookId,
                        Price = b.Price,
                        Quantity = 2
                    })
            };
            PlaceOrderService target1 = new PlaceOrderService(
                placeOrderDbAccess: dbAccessMock.Object,
                signInContext: signInContextUserIdEmptyMock.Object);
            PlaceOrderService target2 = new PlaceOrderService(
               placeOrderDbAccess: dbAccessMock.Object,
               signInContext: signInContextUserIdNullMock.Object);

            // Act

            target1.PlaceOrder(placeOrderDataIn: dto1);
            target2.PlaceOrder(placeOrderDataIn: dto1);

            // Assert

            Assert.True(target1.HasErrors);
            Assert.True(target2.HasErrors);

            Assert.Single(target1.Errors);
            Assert.Single(target2.Errors);

            Assert.Contains(
                expectedSubstring: "unauthorized users cannot place an order",
                actualString: target1.Errors.Single().ErrorMessage);
            Assert.Contains(
                expectedSubstring: "unauthorized users cannot place an order",
                actualString: target2.Errors.Single().ErrorMessage);

            dbAccessMock.Verify(x => x.Add(It.IsAny<Order>()),
                Times.Never);
            dbAccessMock.Verify(x => x.SaveChanges(),
                Times.Never);
        }

        private Book[] GenerateBooks(int num)
        {
            List<Book> list = new List<Book>();
            decimal itemPrice = 10.1M;
            for (int i = 0; i < num; i++)
            {
                string guidString = String.Format("{0:00000000-0000-0000-0000-000000000000}", 1 + i);
                list.Add(new Book
                {
                    BookId = new Guid(guidString),
                    Title = $"book-{1 + i}",
                    Price = itemPrice
                });
                itemPrice += 0.1M;
            }
            return list.ToArray();
        }
    }
}
