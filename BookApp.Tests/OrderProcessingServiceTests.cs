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

        [Fact]
        public void Cannot_Goto_Ready_Status_When_Has_Nonincluded()
        {
            // arrange
            Book book1 = new Book { BookId = new Guid("00000000-0000-0000-0000-000000000001") };
            Book book2 = new Book { BookId = new Guid("00000000-0000-0000-0000-000000000002") };
            var ordersDict = new[]
            {
                new Order
                {
                    OrderId = 1, Status = OrderStatus.New,
                    Lines = new []
                    {
                        new OrderLineItem { BookId = book1.BookId },
                        new OrderLineItem { BookId = book2.BookId }
                    }
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
                    new OrderAssemblyCompletedItemDto { BookId = book2.BookId, Included = false },
                }
            };

            // act
            target.SetOrderStatusToReady(orderAssemblingDto);

            // assert
            Assert.True(target.HasErrors);
            Assert.Single(target.Errors);
            Assert.Contains(
                expectedSubstring: "has non included books",
                actualString: target.Errors.Single().ErrorMessage);
            mock.Verify(
                expression: m => m.SaveOrder(It.IsAny<Order>()),
                times: Times.Never);
        }

        [Fact]
        public void Cannot_Goto_Ready_Status_When_Included_Not_All_Ordered_Books()
        {
            // arrange
            Book book1 = new Book { BookId = new Guid("00000000-0000-0000-0000-000000000001") };
            Book book2 = new Book { BookId = new Guid("00000000-0000-0000-0000-000000000002") };
            var ordersDict = new[]
            {
                new Order
                {
                    OrderId = 1, Status = OrderStatus.New,
                    Lines = new [] 
                    { 
                        new OrderLineItem { BookId = book1.BookId },
                        new OrderLineItem { BookId = book2.BookId }
                    }
                }
            }
            .ToDictionary(o => o.OrderId);

            Mock<IOrderProcessingDbAccess> mock = new Mock<IOrderProcessingDbAccess>();
            mock.Setup(m => m.GetOrderOrigin(It.IsAny<int>()))
                .Returns<int>(orderId => ordersDict.ContainsKey(orderId)
                    ? ordersDict[orderId]
                    : null);
            mock.Setup(m => m.GetOrderLines(It.IsAny<int>()))
                .Returns<int>(orderId => ordersDict.ContainsKey(orderId)
                    ? ordersDict[orderId].Lines
                    : null);

            var target = new OrderProcessingService(orderProcessingDbAccess: mock.Object);

            var orderAssemblingDto = new OrderAssemblingCompletedDto
            {
                OrderId = 1,
                LineItems = new[]
                {
                    new OrderAssemblyCompletedItemDto { BookId = book1.BookId, Included = true }
                }
            };

            // act
            target.SetOrderStatusToReady(orderAssemblingDto);

            // assert
            Assert.True(target.HasErrors);
            Assert.Single(target.Errors);
            Assert.Contains(
                expectedSubstring: "not all books included",
                actualString: target.Errors.Single().ErrorMessage);
            mock.Verify(
                expression: m => m.SaveOrder(It.IsAny<Order>()),
                times: Times.Never);
        }

        [Fact]
        public void Cannot_Goto_Ready_Status_From_Noncorrespond_Status()
        {
            // arrange
            Book book1 = new Book { BookId = new Guid("00000000-0000-0000-0000-000000000001") };
            var order1 = new Order
            {
                OrderId = 1, Status = OrderStatus.New,
                Lines = new[] { new OrderLineItem { BookId = book1.BookId } }
            };
            var order2 = new Order
            {
                OrderId = 2, Status = OrderStatus.Cancelled,
                Lines = new[] { new OrderLineItem { BookId = book1.BookId } }
            };
            var order3 = new Order
            {
                OrderId = 3, Status = OrderStatus.Completed,
                Lines = new[] { new OrderLineItem { BookId = book1.BookId } }
            };
            var order4 = new Order
            {
                OrderId = 4, Status = OrderStatus.Ready,
                Lines = new[] { new OrderLineItem { BookId = book1.BookId } }
            };

            // arrange
            var ordersDict = new[] { order1, order2, order3, order4 }
                .ToDictionary(o => o.OrderId);
            Mock<IOrderProcessingDbAccess> mock = new Mock<IOrderProcessingDbAccess>();
            mock.Setup(m => m.GetOrderOrigin(It.IsAny<int>()))
                .Returns<int>(orderId => ordersDict.ContainsKey(orderId)
                    ? ordersDict[orderId]
                    : null);
            mock.Setup(m => m.GetOrderLines(It.IsAny<int>()))
                .Returns<int>(orderId => ordersDict.ContainsKey(orderId)
                    ? ordersDict[orderId].Lines
                    : null);

            var target1 = new OrderProcessingService(orderProcessingDbAccess: mock.Object);
            var target2 = new OrderProcessingService(orderProcessingDbAccess: mock.Object);
            var target3 = new OrderProcessingService(orderProcessingDbAccess: mock.Object);
            var target4 = new OrderProcessingService(orderProcessingDbAccess: mock.Object);

            var dto1 = new OrderAssemblingCompletedDto
            {
                OrderId = 1,
                LineItems = new[]
                {
                    new OrderAssemblyCompletedItemDto { BookId = book1.BookId, Included = true }
                }
            };
            var dto2 = new OrderAssemblingCompletedDto
            {
                OrderId = 2,
                LineItems = new[]
                {
                    new OrderAssemblyCompletedItemDto { BookId = book1.BookId, Included = true }
                }
            };
            var dto3 = new OrderAssemblingCompletedDto
            {
                OrderId = 3,
                LineItems = new[]
                {
                    new OrderAssemblyCompletedItemDto { BookId = book1.BookId, Included = true }
                }
            };
            var dto4 = new OrderAssemblingCompletedDto
            {
                OrderId = 4,
                LineItems = new[]
                {
                    new OrderAssemblyCompletedItemDto { BookId = book1.BookId, Included = true }
                }
            };

            // act
            target1.SetOrderStatusToReady(dto1);
            target2.SetOrderStatusToReady(dto2);
            target3.SetOrderStatusToReady(dto3);
            target4.SetOrderStatusToReady(dto4);

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
                expectedSubstring: "can set ready status for assembling only",
                actualString: target1.Errors.Single().ErrorMessage);
            Assert.Contains(
                expectedSubstring: "can set ready status for assembling only",
                actualString: target2.Errors.Single().ErrorMessage);
            Assert.Contains(
                expectedSubstring: "can set ready status for assembling only",
                actualString: target3.Errors.Single().ErrorMessage);
            Assert.Contains(
                expectedSubstring: "can set ready status for assembling only",
                actualString: target4.Errors.Single().ErrorMessage);

            mock.Verify(
                expression: m => m.SaveOrder(It.IsAny<Order>()),
                times: Times.Never);
        }

        [Fact]
        public void Can_Goto_Ready_Status()
        {
            // arrange
            Book book1 = new Book { BookId = new Guid("00000000-0000-0000-0000-000000000001") };
            Book book2 = new Book { BookId = new Guid("00000000-0000-0000-0000-000000000002") };
            Order order1 = new Order
            {
                OrderId = 1,
                Status = OrderStatus.Assembling,
                Lines = new[] 
                {
                    new OrderLineItem { BookId = book1.BookId },
                    new OrderLineItem { BookId = book2.BookId }
                }
            };
            var ordersDict = new[] { order1 }
                .ToDictionary(o => o.OrderId);

            Mock<IOrderProcessingDbAccess> mock = new Mock<IOrderProcessingDbAccess>();
            mock.Setup(m => m.GetOrderOrigin(It.IsAny<int>()))
                .Returns<int>(orderId => ordersDict.ContainsKey(orderId)
                    ? ordersDict[orderId]
                    : null);
            mock.Setup(m => m.GetOrderLines(It.IsAny<int>()))
                .Returns<int>(orderId => ordersDict.ContainsKey(orderId)
                    ? ordersDict[orderId].Lines
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
            Assert.False(target.HasErrors);
            Assert.Empty(target.Errors);
            
            mock.Verify(
                expression: m => m.SaveOrder(It.Is<Order>(o=>
                    o.OrderId == order1.OrderId
                    && o.Status == OrderStatus.Ready
                    && o.Lines.Count() == 2
                    && o.Lines.ElementAt(0).BookId == book1.BookId
                    && o.Lines.ElementAt(1).BookId == book2.BookId
                    )),
                times: Times.Once);
        }

        [Fact]
        public void Cannot_Goto_Completed_Status_From_Noncorrespond_Status()
        {
            // arrange
            Book book1 = new Book { BookId = new Guid("00000000-0000-0000-0000-000000000001") };
            var order1 = new Order
            {
                OrderId = 1,
                Status = OrderStatus.New,
                Lines = new[] { new OrderLineItem { BookId = book1.BookId } }
            };
            var order2 = new Order
            {
                OrderId = 2,
                Status = OrderStatus.Assembling,
                Lines = new[] { new OrderLineItem { BookId = book1.BookId } }
            };
            var order3 = new Order
            {
                OrderId = 3,
                Status = OrderStatus.Cancelled,
                Lines = new[] { new OrderLineItem { BookId = book1.BookId } }
            };
            var order4 = new Order
            {
                OrderId = 4,
                Status = OrderStatus.Completed,
                Lines = new[] { new OrderLineItem { BookId = book1.BookId } }
            };

            // arrange
            var ordersDict = new[] { order1, order2, order3, order4 }
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
            target1.SetOrderStatusToCompleted(orderId: order1.OrderId);
            target2.SetOrderStatusToCompleted(orderId: order2.OrderId);
            target3.SetOrderStatusToCompleted(orderId: order3.OrderId);
            target4.SetOrderStatusToCompleted(orderId: order4.OrderId);

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
                expectedSubstring: "can complete ready orders only",
                actualString: target1.Errors.Single().ErrorMessage);
            Assert.Contains(
                expectedSubstring: "can complete ready orders only",
                actualString: target2.Errors.Single().ErrorMessage);
            Assert.Contains(
                expectedSubstring: "can complete ready orders only",
                actualString: target3.Errors.Single().ErrorMessage);
            Assert.Contains(
                expectedSubstring: "can complete ready orders only",
                actualString: target4.Errors.Single().ErrorMessage);

            mock.Verify(
                expression: m => m.SaveOrder(It.IsAny<Order>()),
                times: Times.Never);
        }

        [Fact]
        public void Cannot_Goto_Completed_Status_When_Order_Not_Exist_In_Db()
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

            // act
            target.SetOrderStatusToCompleted(orderId: 4);

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
        public void Can_Goto_Completed_Status()
        {
            // arrange
            Book book1 = new Book { BookId = new Guid("00000000-0000-0000-0000-000000000001") };
            Order order1 = new Order
            {
                OrderId = 1,
                Status = OrderStatus.Ready,
                Lines = new[]
                {
                    new OrderLineItem { BookId = book1.BookId }
                }
            };
            var ordersDict = new[] { order1 }
                .ToDictionary(o => o.OrderId);

            Mock<IOrderProcessingDbAccess> mock = new Mock<IOrderProcessingDbAccess>();
            mock.Setup(m => m.GetOrderOrigin(It.IsAny<int>()))
                .Returns<int>(orderId => ordersDict.ContainsKey(orderId)
                    ? ordersDict[orderId]
                    : null);
            mock.Setup(m => m.GetOrderLines(It.IsAny<int>()))
                .Returns<int>(orderId => ordersDict.ContainsKey(orderId)
                    ? ordersDict[orderId].Lines
                    : null);

            var target = new OrderProcessingService(orderProcessingDbAccess: mock.Object);

            // act
            target.SetOrderStatusToCompleted(orderId: 1);

            // assert
            Assert.False(target.HasErrors);
            Assert.Empty(target.Errors);

            mock.Verify(
                expression: m => m.SaveOrder(It.Is<Order>(o =>
                    o.OrderId == order1.OrderId
                    && o.Status == OrderStatus.Completed
                    )),
                times: Times.Once);
        }
    }
}
