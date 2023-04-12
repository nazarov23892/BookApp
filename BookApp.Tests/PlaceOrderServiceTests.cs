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

namespace BookApp.Tests
{
    public class PlaceOrderServiceTests
    {
        [Fact]
        public void Cannot_PlaceOrder_When_Empty_or_Null_Lines()
        {
            // Arrange
            Mock<IPlaceOrderDbAccess> mock = new Mock<IPlaceOrderDbAccess>();

            PlaceOrderService target1 = new PlaceOrderService(placeOrderDbAccess: mock.Object);
            PlaceOrderService target2 = new PlaceOrderService(placeOrderDbAccess: mock.Object);

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

            mock.Verify(x => x.SaveChanges(),
                Times.Never);
            mock.Verify(x => x.Add(It.IsAny<Order>()),
                Times.Never);
        }

        [Fact]
        public void Cannot_PlaceOrder_When_Firstname_Empty()
        {
            // Arrange
            Mock<IPlaceOrderDbAccess> mock = new Mock<IPlaceOrderDbAccess>();

            PlaceOrderService target1 = new PlaceOrderService(placeOrderDbAccess: mock.Object);

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
            mock.Verify(x => x.Add(It.IsAny<Order>()),
                Times.Never);
            mock.Verify(x => x.SaveChanges(),
                Times.Never);
        }

        [Fact]
        public void Cannot_PlaceOrder_When_Lastname_Empty()
        {
            // Arrange
            Mock<IPlaceOrderDbAccess> mock = new Mock<IPlaceOrderDbAccess>();
            PlaceOrderService target1 = new PlaceOrderService(placeOrderDbAccess: mock.Object);

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
            mock.Verify(x => x.Add(It.IsAny<Order>()),
                Times.Never);
            mock.Verify(x => x.SaveChanges(),
                Times.Never);
        }

        [Fact]
        public void Cannot_PlaceOrder_When_Phonenumber_Empty()
        {
            // Arrange
            Mock<IPlaceOrderDbAccess> mock = new Mock<IPlaceOrderDbAccess>();
            PlaceOrderService target1 = new PlaceOrderService(placeOrderDbAccess: mock.Object);

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
            mock.Verify(x => x.Add(It.IsAny<Order>()),
                Times.Never);
            mock.Verify(x => x.SaveChanges(),
                Times.Never);
        }

        [Fact]
        public void Cannot_PlaceOrder_When_Quantity_is_less_or_equal_zero()
        {
            // Arrange
            Mock<IPlaceOrderDbAccess> mock = new Mock<IPlaceOrderDbAccess>();
            PlaceOrderService target1 = new PlaceOrderService(placeOrderDbAccess: mock.Object);
            PlaceOrderService target2 = new PlaceOrderService(placeOrderDbAccess: mock.Object);

            var dto1 = new PlaceOrderDto
            {
                Firstname = "firstname",
                Lastname = "lastname",
                PhoneNumber = "111",
                Lines = new[]
                {
                    new PlaceOrderLineItemDto
                    {
                        BookId = new Guid("00000000-0000-0000-0000-000000000001"),
                        Quantity = 0,
                        Price = 10.1M
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
                        BookId = new Guid("00000000-0000-0000-0000-000000000001"),
                        Quantity = -1,
                        Price = 10.1M
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
                value: "invalid quantity value",
                comparisonType: StringComparison.OrdinalIgnoreCase));
            Assert.True(error2.ErrorMessage.Contains(
                value: "invalid quantity value",
                comparisonType: StringComparison.OrdinalIgnoreCase));

            mock.Verify(x => x.Add(It.IsAny<Order>()),
                Times.Never);
            mock.Verify(x => x.SaveChanges(),
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
                    Quantity = 1
                },
                new PlaceOrderLineItemDto
                {
                    BookId = dbBook2.BookId,
                    Title = dbBook2.Title,
                    Quantity = 1
                },
                new PlaceOrderLineItemDto
                {
                    BookId = missingId,
                    Title = "title",
                    Quantity = 1
                }
            };

            // Arrange
            Mock<IPlaceOrderDbAccess> mock = new Mock<IPlaceOrderDbAccess>();
            mock.Setup(m => m.FindBooksByIds(It.IsAny<IEnumerable<Guid>>()))
               .Returns<IEnumerable<Guid>>((ids) => new[] { dbBook1, dbBook2 }
                        .ToDictionary(b=>b.BookId));
              
            var dto1 = new PlaceOrderDto
            {
                Firstname = "firstname",
                Lastname = "lastname",
                PhoneNumber = "111",
                Lines = lines
            };
            PlaceOrderService target1 = new PlaceOrderService(placeOrderDbAccess: mock.Object);

            // Act

            Action act = () => target1.PlaceOrder(placeOrderDataIn: dto1);
            var exception = Assert.Throws<InvalidOperationException>(act);

            // Assert

            Assert.NotNull(exception);
            Assert.True(exception.Message.Contains(
                value: "A placing of order failed: book id",
                comparisonType: StringComparison.OrdinalIgnoreCase));

            mock.Verify(x => x.Add(It.IsAny<Order>()),
                Times.Never);
            mock.Verify(x => x.SaveChanges(),
                Times.Never);
        }
    }
}
