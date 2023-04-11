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
        public void Cannot_PlaceOrder_When_Empty_Lines()
        {
            // Arrange
            Mock<IPlaceOrderDbAccess> mock = new Mock<IPlaceOrderDbAccess>();

            PlaceOrderDto dto = new PlaceOrderDto
            {
                Firstname = "firstname",
                Lastname = "lastname",
                PhoneNumber = "111",
                Lines = Enumerable.Empty<PlaceOrderLineItemDto>()
            };
            PlaceOrderService target = new PlaceOrderService(placeOrderDbAccess: mock.Object);
            
            // Act
            var orderId = target.PlaceOrder(placeOrderDataIn: dto);

            // Assert
            Assert.True(target.HasErrors);
            Assert.Equal(0, orderId);
            Assert.Single(target.Errors);
            var error = target.Errors.First();
            Assert.True(error.ErrorMessage.Contains(
                value: "No items in your order.", 
                comparisonType: StringComparison.OrdinalIgnoreCase));
            mock.Verify(x => x.SaveChanges(),
                Times.Never);
            mock.Verify(x => x.Add(It.IsAny<Order>()),
                Times.Never);
        }
    }
}
