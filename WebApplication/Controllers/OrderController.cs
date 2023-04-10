using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceLayer.CartServices;

namespace WebApplication.Controllers
{
    public class OrderController : Controller
    {
        private readonly  ICartService cartService;

        public OrderController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        public ViewResult Checkout()
        {
            return View(model: cartService.Lines);
        }
    }
}
