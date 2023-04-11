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
using DataLayer.Entities;

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
            var orderId1 = target1.PlaceOrder(placeOrderDataIn: new PlaceOrderDto
            {
                Firstname = "firstname",
                Lastname = "lastname",
                PhoneNumber = "111",
                Lines = Enumerable.Empty<PlaceOrderLineItemDto>()
            });
            var orderId2 = target2.PlaceOrder(placeOrderDataIn: new PlaceOrderDto
            {
                Firstname = "firstname",
                Lastname = "lastname",
                PhoneNumber = "111",
                Lines = null
            });


            // Assert
            Assert.True(target1.HasErrors);
            Assert.True(target2.HasErrors);

            Assert.Equal(0, orderId1);
            Assert.Equal(0, orderId2);

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
            var orderId1 = target1.PlaceOrder(placeOrderDataIn: new PlaceOrderDto
            {
                Firstname = string.Empty,
                Lastname = "lastname",
                PhoneNumber = "111",
                Lines = new[] 
                { 
                    new PlaceOrderLineItemDto { BookId = Guid.Parse("1"), Quantity = 1, Price = 10.1M }, 
                    new PlaceOrderLineItemDto { BookId = Guid.Parse("2"), Quantity = 1, Price = 10.2M }, 
                }
            });


            // Assert
            Assert.True(target1.HasErrors);
            Assert.Equal(0, orderId1);
            Assert.Single(target1.Errors);
            var error1 = target1.Errors.First();

            mock.Verify(x => x.Add(It.IsAny<Order>()),
                Times.Never);
            mock.Verify(x => x.SaveChanges(),
                Times.Never);
        }
    }
}
