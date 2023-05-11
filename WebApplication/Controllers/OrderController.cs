using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL;
using Microsoft.AspNetCore.Authorization;
using BookApp.BLL.Services.Orders;
using BookApp.BLL.Services.Cart;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Authorize(Roles = DomainConstants.UsersRoleName)]
    public class OrderController : Controller
    {
        private readonly ICartService cartService;
        private readonly IPlaceOrderService placeOrderService;
        private readonly IDisplayOrderService displayOrderService;
        private readonly IOrderManageService orderManageService;

        public OrderController(
            ICartService cartService,
            IPlaceOrderService placeOrderService,
            IDisplayOrderService displayOrderService,
            IOrderManageService orderManageService)
        {
            this.cartService = cartService;
            this.placeOrderService = placeOrderService;
            this.displayOrderService = displayOrderService;
            this.orderManageService = orderManageService;
        }

        [HttpGet]
        public ViewResult Checkout()
        {
            var dto = new PlaceOrderDto
            {
                Firstname = "",
                Lastname = "",
                PhoneNumber = "",
                Lines = cartService.Lines
                    .Select(l => new PlaceOrderLineItemDto
                    {
                        BookId = l.Book.BookId,
                        Title = l.Book.Title,
                        Price = l.Book.Price,
                        Quantity = l.Quantity
                    })
            };
            return View(model: dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PlaceOrder(PlaceOrderDto inDto)
        {
            if (!ModelState.IsValid)
            {
                goto exit_errors;
            }
            int orderId = placeOrderService.PlaceOrder(placeOrderDataIn: inDto);
            if (placeOrderService.HasErrors)
            {
                foreach (var error in placeOrderService.Errors)
                {
                    ModelState.AddModelError(
                        key: "",
                        errorMessage: error.ErrorMessage);
                }
                goto exit_errors;
            }
            return RedirectToAction(
                actionName: nameof(this.Success),
                controllerName: "Order",
                routeValues: new { id = orderId });

        exit_errors:
            inDto.Lines = cartService.Lines
                .Select(l => new PlaceOrderLineItemDto
                {
                    BookId = l.Book.BookId,
                    Title = l.Book.Title,
                    Price = l.Book.Price,
                    Quantity = l.Quantity
                });
            return View(
                viewName: nameof(Checkout),
                model: inDto);
        }

        public ViewResult Success(int id)
        {
            return View();
        }

        public ViewResult List()
        {
            var orders = displayOrderService.GetOrders();
            return View(model: orders);
        }

        public IActionResult Details(int id)
        {
            var order = displayOrderService.GetItem(orderId: id);
            if (order == null)
            {
                return NotFound();
            }
            return View(model: order);
        }

        [HttpPost]
        public IActionResult Cancel(int id)
        {
            orderManageService.Cancel(orderId: id);
            if (orderManageService.HasErrors)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var error in orderManageService.Errors)
                {
                    stringBuilder.AppendLine(error.ErrorMessage);
                }
                TempData.WriteAlertMessage(
                    messageText: stringBuilder.ToString(),
                    messageType: ViewAlertMessageType.Danger);
            }
            return RedirectToAction(
                actionName: nameof(this.Details),
                controllerName: "Order",
                routeValues: new { id = id });
        }
    }
}
