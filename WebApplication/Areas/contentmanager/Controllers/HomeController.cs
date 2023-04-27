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
        private readonly IBookEditService bookEditService;

        public HomeController(
            IBookCatalogService bookCatalogService,
            IBookEditService bookEditService)
        {
            this.bookCatalogService = bookCatalogService;
            this.bookEditService = bookEditService;
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

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BookCreateDto newBook)
        {
            if (!ModelState.IsValid)
            {
                goto error_exit;
            }
            var bookId = bookEditService.CreateBook(newBook);
            if (bookEditService.HasErrors)
            {
                foreach (var error in bookEditService.Errors)
                {
                    ModelState.AddModelError(key: "", errorMessage: error.ErrorMessage);
                }
                goto error_exit;
            }
            return RedirectToAction(
                actionName: nameof(this.Index),
                controllerName: "Home");

        error_exit:
            return View(model: newBook);
        }
    }
}
