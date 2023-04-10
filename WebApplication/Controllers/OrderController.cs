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

        public OrderController(ICartService cartService)
        {
            this.cartService = cartService;
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
    }
}
