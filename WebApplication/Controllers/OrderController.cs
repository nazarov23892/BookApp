using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceLayer.CartServices;
using ServiceLayer.OrderServices;

namespace WebApplication.Controllers
{
    public class OrderController : Controller
    {
        private readonly  ICartService cartService;
        private readonly IPlaceOrderService placeOrderService;

        public OrderController(
            ICartService cartService,
            IPlaceOrderService placeOrderService)
        {
            this.cartService = cartService;
            this.placeOrderService = placeOrderService;
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
    }
}
