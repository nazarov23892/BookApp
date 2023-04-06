using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceLayer.BookCatalogServices;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookCatalogService bookService;

        public HomeController(IBookCatalogService bookService)
        {
            this.bookService = bookService;
        }

        public IActionResult Index(PageOptionsIn pageOptions)
        {
            var bookListDto = bookService.GetList(pageOptionsIn: pageOptions);
            return View(model: bookListDto);
        }

        public ViewResult Details(Guid id)
        {
            var bookDetailsDto = bookService.GetItem(bookId: id);
            return View(model: bookDetailsDto);
        }
    }
}
