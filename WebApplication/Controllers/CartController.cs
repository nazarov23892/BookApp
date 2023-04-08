using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceLayer.CartServices;

namespace WebApplication.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly IBookForCartService bookCartService;

        public CartController(ICartService cartService,
            IBookForCartService bookCartService)
        {
            this.cartService = cartService;
            this.bookCartService = bookCartService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(Guid id)
        {
            BookForCartDto bookDto = bookCartService.GetItem(id);
            if (bookDto != null)
            {
                cartService.Add(bookDto);
            }
            return RedirectToAction(
                actionName: nameof(this.Index),
                controllerName: "Home");
        }

        public IActionResult Index()
        {
            return View(model: cartService.Lines);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(Guid id)
        {
            cartService.Remove(bookId: id);
            return RedirectToAction(
                actionName: nameof(this.Index),
                controllerName: "Cart");
        }

        [HttpGet]
        public ViewResult Change(Guid id)
        {
            ViewBag.EditId = id;
            return View(
                viewName: "Index",
                model: cartService.Lines);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Change(CartLineChangeQuantityDto changesDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EditId = changesDto.BookId;
                return View(
                    viewName: "Index",
                    model: cartService.Lines);
            }
            cartService.SetQuantity(
                bookId: changesDto.BookId,
                quantity: changesDto.Quantity);

            return RedirectToAction(
                actionName: nameof(this.Index),
                controllerName: "Cart");
        }
    }
}
