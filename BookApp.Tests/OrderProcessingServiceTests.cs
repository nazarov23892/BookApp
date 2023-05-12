using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Services.OrderProcessing;
using BookApp.BLL.Services.OrderProcessing.Concrete;
using BookApp.BLL.Entities;
using Xunit;
using Moq;

namespace BookApp.Tests
{
    public class OrderProcessingServiceTests
    {
        [Fact]
        public void Cannot_Goto_Assembly_Status_When_Order_Not_Exist_In_Db()
        {
            // arrange
            var ordersDict = new[]
            {
                new Order { OrderId = 1, Status = OrderStatus.New }
            }
            .ToDictionary(o => o.OrderId);

            Mock<IOrderProcessingDbAccess> mock = new Mock<IOrderProcessingDbAccess>();
            mock.Setup(m => m.GetOrderOrigin(It.IsAny<int>()))
                .Returns<int>(orderId => ordersDict.ContainsKey(orderId)
                    ? ordersDict[orderId]
                    : null);

            var target = new OrderProcessingService(orderProcessingDbAccess: mock.Object);

            // act
            target.SetOrderStatusToAssembling(orderId: 0);

            // assert
            Assert.True(target.HasErrors);
            Assert.Single(target.Errors);
            Assert.Contains(
                expectedSubstring: "order not found",
                actualString: target.Errors.Single().ErrorMessage);
            mock.Verify(
                expression: m => m.SaveOrder(It.IsAny<Order>()),
                times: Times.Never);
        }

        [Fact]
        public void Cannot_Goto_Assembling_Status_From_Noncorrespond_Status()
        {
            // arrange
            var ordersDict = new[]
            {
                new Order { OrderId = 1, Status = OrderStatus.Assembling },
                new Order { OrderId = 2, Status = OrderStatus.Cancelled },
                new Order { OrderId = 3, Status = OrderStatus.Completed },
                new Order { OrderId = 4, Status = OrderStatus.Ready }
            }
            .ToDictionary(o => o.OrderId);
            Mock<IOrderProcessingDbAccess> mock = new Mock<IOrderProcessingDbAccess>();
            mock.Setup(m => m.GetOrderOrigin(It.IsAny<int>()))
                .Returns<int>(orderId => ordersDict.ContainsKey(orderId)
                    ? ordersDict[orderId]
                    : null);

            var target1 = new OrderProcessingService(orderProcessingDbAccess: mock.Object);
            var target2 = new OrderProcessingService(orderProcessingDbAccess: mock.Object);
            var target3 = new OrderProcessingService(orderProcessingDbAccess: mock.Object);
            var target4 = new OrderProcessingService(orderProcessingDbAccess: mock.Object);

            // act
            target1.SetOrderStatusToAssembling(orderId: 1);
            target2.SetOrderStatusToAssembling(orderId: 2);
            target3.SetOrderStatusToAssembling(orderId: 3);
            target4.SetOrderStatusToAssembling(orderId: 4);

            // assert
            Assert.True(target1.HasErrors);
            Assert.True(target2.HasErrors);
            Assert.True(target3.HasErrors);
            Assert.True(target4.HasErrors);

            Assert.Single(target1.Errors);
            Assert.Single(target2.Errors);
            Assert.Single(target3.Errors);
            Assert.Single(target4.Errors);

            Assert.Contains(
                expectedSubstring: "can assembly new orders only",
                actualString: target1.Errors.Single().ErrorMessage);
            Assert.Contains(
                expectedSubstring: "can assembly new orders only",
                actualString: target2.Errors.Single().ErrorMessage);
            Assert.Contains(
                expectedSubstring: "can assembly new orders only",
                actualString: target3.Errors.Single().ErrorMessage);
            Assert.Contains(
                expectedSubstring: "can assembly new orders only",
                actualString: target4.Errors.Single().ErrorMessage);

            mock.Verify(
                expression: m => m.SaveOrder(It.IsAny<Order>()),
                times: Times.Never);
        }

        [Fact]
        public void Can_Goto_Assembly_Status()
        {
            // arrange
            var ordersDict = new[]
            {
                new Order { OrderId = 1, Status = OrderStatus.New }
            }
            .ToDictionary(o => o.OrderId);

            Mock<IOrderProcessingDbAccess> mock = new Mock<IOrderProcessingDbAccess>();
            mock.Setup(m => m.GetOrderOrigin(It.IsAny<int>()))
                .Returns<int>(orderId => ordersDict.ContainsKey(orderId)
                    ? ordersDict[orderId]
                    : null);

            var target = new OrderProcessingService(orderProcessingDbAccess: mock.Object);

            // act
            target.SetOrderStatusToAssembling(orderId: 1);

            // assert
            Assert.False(target.HasErrors);
            Assert.Empty(target.Errors);
            mock.Verify(
                expression: m => m.SaveOrder(It.Is<Order>(o => o.OrderId == 1
                    && o.Status == OrderStatus.Assembling)),
                times: Times.Once);
        }

        [Fact]
        public void Cannot_Goto_Ready_Status_When_Order_Not_Exist_In_Db()
        {
            // arrange
            Book book1 = new Book { BookId = new Guid("00000000-0000-0000-0000-000000000001") };
            var ordersDict = new[]
            {
                new Order
                {
                    OrderId = 1, Status = OrderStatus.New,
                    Lines = new [] { new OrderLineItem { BookId = book1.BookId } }
                }
            }
            .ToDictionary(o => o.OrderId);

            Mock<IOrderProcessingDbAccess> mock = new Mock<IOrderProcessingDbAccess>();
            mock.Setup(m => m.GetOrderOrigin(It.IsAny<int>()))
                .Returns<int>(orderId => ordersDict.ContainsKey(orderId)
                    ? ordersDict[orderId]
                    : null);

            var target = new OrderProcessingService(orderProcessingDbAccess: mock.Object);

            var orderAssemblingDto = new OrderAssemblingCompletedDto
            {
                OrderId = 2,
                LineItems = new[]
                {
                    new OrderAssemblyCompletedItemDto{BookId = book1.BookId, Included = true }
                }
            };

            // act
            target.SetOrderStatusToReady(orderAssemblingDto);

            // assert
            Assert.True(target.HasErrors);
            Assert.Single(target.Errors);
            Assert.Contains(
                expectedSubstring: "order not found",
                actualString: target.Errors.Single().ErrorMessage);
            mock.Verify(
                expression: m => m.SaveOrder(It.IsAny<Order>()),
                times: Times.Never);
        }

        [Fact]
        public void Cannot_Goto_Ready_Status_When_Order_Not_Contains_Included_Book()
        {
            // arrange
            Book book1 = new Book { BookId = new Guid("00000000-0000-0000-0000-000000000001") };
            Book book2 = new Book { BookId = new Guid("00000000-0000-0000-0000-000000000002") };
            var ordersDict = new[]
            {
                new Order
                {
                    OrderId = 1, Status = OrderStatus.New,
                    Lines = new [] { new OrderLineItem { BookId = book1.BookId } }
                }
            }
            .ToDictionary(o => o.OrderId);

            Mock<IOrderProcessingDbAccess> mock = new Mock<IOrderProcessingDbAccess>();
            mock.Setup(m => m.GetOrderOrigin(It.IsAny<int>()))
                .Returns<int>(orderId => ordersDict.ContainsKey(orderId)
                    ? ordersDict[orderId]
                    : null);

            var target = new OrderProcessingService(orderProcessingDbAccess: mock.Object);

            var orderAssemblingDto = new OrderAssemblingCompletedDto
            {
                OrderId = 1,
                LineItems = new[]
                {
                    new OrderAssemblyCompletedItemDto { BookId = book1.BookId, Included = true },
                    new OrderAssemblyCompletedItemDto { BookId = book2.BookId, Included = true }
                }
            };

            // act
            target.SetOrderStatusToReady(orderAssemblingDto);

            // assert
            Assert.True(target.HasErrors);
            Assert.Single(target.Errors);
            Assert.Contains(
                expectedSubstring: "order does not contain the book",
                actualString: target.Errors.Single().ErrorMessage);
            mock.Verify(
                expression: m => m.SaveOrder(It.IsAny<Order>()),
                times: Times.Never);
        }
    }
}
