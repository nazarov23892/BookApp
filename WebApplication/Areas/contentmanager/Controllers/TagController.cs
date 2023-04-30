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

        public TagController(
            ITagDbAccess tagDbAccess)
        {
            this.tagDbAccess = tagDbAccess;
        }

        public IActionResult Index()
        {
            var tags = tagDbAccess.GetTags();
            return View(model: tags);
        }
    }
}
