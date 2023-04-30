using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApp.BLL.Services.Tags;

namespace WebApplication.Areas.contentmanager.Controllers
{
    [Area(areaName: "contentmanager")]
    public class TagController : Controller
    {
        private readonly ITagDbAccess tagDbAccess;
        private readonly ITagService tagService;

        public TagController(
            ITagDbAccess tagDbAccess,
            ITagService tagService)
        {
            this.tagDbAccess = tagDbAccess;
            this.tagService = tagService;
        }

        public IActionResult Index()
        {
            var tags = tagDbAccess.GetTags();
            return View(model: tags);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TagCreateDto newTag)
        {
            if (!ModelState.IsValid)
            {
                goto error_exit;
            }
            tagService.CreateTag(newTag);
            if (tagService.HasErrors)
            {
                foreach (var error in tagService.Errors)
                {
                    ModelState.AddModelError(key: "", errorMessage: error.ErrorMessage);
                }
                goto error_exit;
            }
            return RedirectToAction(
                actionName: nameof(this.Index),
                controllerName: "Tag",
                routeValues: new { area = "contentmanager" });

        error_exit:
            return View(model: newTag);
        }
    }
}
