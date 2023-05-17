using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApp.BLL.Services.Authors;

namespace WebApplication.Areas.contentmanager.Controllers
{
    [Area(areaName: "contentmanager")]
    public class AuthorController : Controller
    {
        private readonly IAuthorService authorService;

        public AuthorController(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        public IActionResult Index(PageOptionsIn pageOptions)
        {
            var authorsDto = authorService.GetAuthors(pageOptions);
            return View(model: authorsDto);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AuthorCreateDto author)
        {
            if (!ModelState.IsValid)
            {
                goto error_exit;
            }
            authorService.CreateAuthor(newAuthor: author);
            if (authorService.HasErrors)
            {
                foreach (var error in authorService.Errors)
                {
                    ModelState.AddModelError(key: "", errorMessage: error.ErrorMessage);
                }
                goto error_exit;
            }
            return RedirectToAction(
                actionName: nameof(this.Index),
                controllerName: "Author");

        error_exit:
            return View(model: author);
        }
    }
}
