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
    }
}
