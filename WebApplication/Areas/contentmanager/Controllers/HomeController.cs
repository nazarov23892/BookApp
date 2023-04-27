using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApp.BLL.Services.BookCatalog;


namespace WebApplication.Areas.contentmanager.Controllers
{
    [Area(areaName: "contentmanager")]
    public class HomeController : Controller
    {
        private readonly IBookCatalogService bookCatalogService;

        public HomeController(IBookCatalogService bookCatalogService)
        {
            this.bookCatalogService = bookCatalogService;
        }

        public IActionResult Index(PageOptionsIn pageOptions)
        {
            var items = bookCatalogService.GetList(pageOptionsIn: pageOptions);
            return View(model: items);
        }

        public IActionResult Details(Guid id)
        {
            BookDetailsDto bookDto = bookCatalogService.GetItem(bookId: id);
            if (bookDto == null)
            {
                return NotFound();
            }
            return View(model: bookDto);
        }
    }
}
