using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApp.BLL.Services.BookCatalog;
using BookApp.BLL.Services.BookManage;
using BookApp.BLL.Services.BookManageAuthors;
using WebApplication.Models;
using System.Text;

namespace WebApplication.Areas.contentmanager.Controllers
{
    [Area(areaName: "contentmanager")]
    public class HomeController : Controller
    {
        private readonly IBookCatalogService bookCatalogService;
        private readonly IBookEditService bookEditService;
        private readonly IBookManageAuthorsService bookManageAuthorsService;
        private readonly IBookEditDbAccess bookEditDbAccess;
        private readonly IBookManageAuthorsDbAccess bookManageAuthorsDbAccess;

        public HomeController(
            IBookCatalogService bookCatalogService,
            IBookEditService bookEditService,
            IBookManageAuthorsService bookManageAuthorsService,
            IBookEditDbAccess bookEditDbAccess,
            IBookManageAuthorsDbAccess bookManageAuthorsDbAccess)
        {
            this.bookCatalogService = bookCatalogService;
            this.bookEditService = bookEditService;
            this.bookManageAuthorsService = bookManageAuthorsService;
            this.bookEditDbAccess = bookEditDbAccess;
            this.bookManageAuthorsDbAccess = bookManageAuthorsDbAccess;
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

        [HttpGet]
        public IActionResult EditAuthors(Guid id, bool? editMode = null)
        {
            BookAuthorsLinkOrderDto bookDto = bookManageAuthorsDbAccess.GetBookForEditAuthors(bookId: id);
            if (bookDto == null)
            {
                return NotFound();
            }
            ViewBag.editMode = editMode;
            return View(model: bookDto);
        }

        [HttpPost]
        public IActionResult EditAuthors(BookAuthorLinksOrderEditedDto authorLinksDto)
        {
            if (!ModelState.IsValid)
            {
                goto error_exit;
            }
            bookManageAuthorsService.ChangeAuthorLinksOrder(authorLinksDto);
            if (bookManageAuthorsService.HasErrors)
            {
                foreach (var error in bookManageAuthorsService.Errors)
                {
                    ModelState.AddModelError(key: "", errorMessage: error.ErrorMessage);
                }
                goto error_exit;
            }

            return RedirectToAction(
                actionName: nameof(this.EditAuthors),
                controllerName: "Home",
                routeValues: new
                {
                    id = authorLinksDto.BookId,
                    area = "contentmanager",
                    editMode = false
                });

        error_exit:
            BookAuthorsLinkOrderDto bookDto = bookManageAuthorsDbAccess.GetBookForEditAuthors(bookId: authorLinksDto.BookId);
            if (bookDto == null)
            {
                return NotFound();
            }
            ViewBag.editMode = true;
            return View(viewName: "EditAuthors", model: bookDto);
        }

        [HttpGet]
        public IActionResult EditTags(Guid id)
        {
            BookWithTagsDto dto = bookEditDbAccess.GetBookForEditTags(bookId: id);
            if (dto == null)
            {
                return NotFound();
            }
            return View(model: dto);
        }

        [HttpGet]
        public IActionResult EditDescription(Guid id)
        {
            BookDescriptionForEditDto bookDescriptionDto = bookEditDbAccess.GetBookForEditDescription(bookId: id);
            if (bookDescriptionDto == null)
            {
                return NotFound();
            }
            return View(model: bookDescriptionDto);
        }

        [HttpPost]
        public IActionResult EditDescription(BookDescriptionEditedDto bookDescriptionDto)
        {
            if (!ModelState.IsValid)
            {
                goto error_exit;
            }
            bookEditService.SetDescription(bookDescriptionDto);
            if (bookEditService.HasErrors)
            {
                foreach (var error in bookEditService.Errors)
                {
                    ModelState.AddModelError(key: "", errorMessage: error.ErrorMessage);
                }
                goto error_exit;
            }
            return RedirectToAction(
                actionName: nameof(this.Details),
                controllerName: "Home",
                routeValues: new
                {
                    area = "contentmanager",
                    id = bookDescriptionDto.BookId
                });

        error_exit:
            var bookForEditDto = bookEditDbAccess.GetBookForEditDescription(bookId: bookDescriptionDto.BookId);
            if (bookForEditDto == null)
            {
                return NotFound();
            }
            bookForEditDto.Description = bookDescriptionDto.Description;
            return View(model: bookForEditDto);
        }

        [HttpGet]
        public IActionResult AddAuthors(Guid id)
        {
            BookAuthorsToAddDto dto = bookManageAuthorsDbAccess.GetAuthorsForAdd(bookId: id);
            if (dto == null)
            {
                return NotFound();
            }
            return View(model: dto);
        }

        [HttpPost]
        public IActionResult AddAuthor(BookAddAuthorDto addAuthorDto)
        {
            if (!ModelState.IsValid)
            {
                goto exit_point;
            }
            bookManageAuthorsService.AddAuthor(addAuthorDto);
            if (bookManageAuthorsService.HasErrors)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var error in bookManageAuthorsService.Errors)
                {
                    stringBuilder.AppendLine(error.ErrorMessage);
                }
                TempData.WriteAlertMessage(
                    messageText: stringBuilder.ToString(),
                    messageType: ViewAlertMessageType.Danger);
                goto exit_point;
            }

        exit_point:
            return RedirectToAction(
                actionName: "AddAuthors",
                controllerName: "Home",
                routeValues: new
                {
                    id = addAuthorDto.BookId,
                    area = "contentmanager"
                });
        }

        [HttpPost]
        public IActionResult RemoveAuthor(BookRemoveAuthorDto bookAuthorDto)
        {
            bookManageAuthorsService.RemoveAuthor(bookAuthorDto);
            if (bookManageAuthorsService.HasErrors)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var error in bookManageAuthorsService.Errors)
                {
                    stringBuilder.AppendLine(error.ErrorMessage);
                }
                TempData.WriteAlertMessage(
                    messageText: stringBuilder.ToString(),
                    messageType: ViewAlertMessageType.Danger);
            }
            return RedirectToAction(
                actionName: nameof(this.EditAuthors),
                controllerName: "Home",
                routeValues: new
                {
                    id = bookAuthorDto.BookId,
                    area = "contentmanager"
                });
        }

        [HttpGet]
        public IActionResult AddTag(Guid id)
        {
            var bookDto = bookEditDbAccess.GetTagsForAdd(bookId: id);
            if (bookDto == null)
            {
                return NotFound();
            }
            return View(model: bookDto);
        }

        [HttpPost]
        public IActionResult AddTag(BookAddTagDto bookTagDto)
        {
            if (!ModelState.IsValid)
            {
                goto exit_point;
            }
            bookEditService.AddTag(bookTagDto);
            if (bookEditService.HasErrors)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var error in bookEditService.Errors)
                {
                    stringBuilder.AppendLine(error.ErrorMessage);
                }
                TempData.WriteAlertMessage(
                    messageText: stringBuilder.ToString(),
                    messageType: ViewAlertMessageType.Danger);
                goto exit_point;
            }

        exit_point:
            return RedirectToAction(
                actionName: nameof(this.EditTags),
                controllerName: "Home",
                routeValues: new { area = "contentmanager", id = bookTagDto.BookId });
        }

        [HttpPost]
        public IActionResult RemoveTag(BookRemoveTagDto bookTagDto)
        {
            bookEditService.RemoveTag(bookTagDto);
            if (bookEditService.HasErrors)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var error in bookEditService.Errors)
                {
                    stringBuilder.AppendLine(error.ErrorMessage);
                }
                TempData.WriteAlertMessage(
                    messageText: stringBuilder.ToString(),
                    messageType: ViewAlertMessageType.Danger);
            }
            return RedirectToAction(
                actionName: nameof(this.EditTags),
                controllerName: "Home",
                routeValues: new
                {
                    id = bookTagDto.BookId,
                    area = "contentmanager"
                });
        }
    }
}
