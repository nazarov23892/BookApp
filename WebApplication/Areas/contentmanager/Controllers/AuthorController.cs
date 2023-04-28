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

        public IActionResult Index()
        {
            IEnumerable<AuthorListItemDto> authors = authorService.GetAuthors();
            return View(model: authors);
        }
    }
}
