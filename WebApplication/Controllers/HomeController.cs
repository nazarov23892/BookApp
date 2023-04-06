using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceLayer.BookServices;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookCatalogService bookService;

        public HomeController(IBookCatalogService bookService)
        {
            this.bookService = bookService;
        }

        public IActionResult Index()
        {
            var books = bookService.GetList();
            return View(model: books);
        }
    }
}
