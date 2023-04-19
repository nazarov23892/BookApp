using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;
using WebApplication.Infrastructure;
using WebApplication.ViewModels;
using System.ComponentModel.DataAnnotations;

using BookApp.BLL.Services.Cart;

namespace WebApplication.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly IBookForCartDbAccess bookCartService;

        public CartController(
            ICartService cartService,
            IBookForCartDbAccess bookCartService)
        {
            this.cartService = cartService;
            this.bookCartService = bookCartService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(Guid id)
        {
            BookForCartDto bookDto = bookCartService.GetItem(id);
            if (bookDto == null)
            {
                TempData.WriteAlertMessage(messageText: "error: book not found");
                goto exit_point;
            }
            cartService.Add(bookDto);
            if (cartService.HasErrors
                && cartService.Errors.FirstOrDefault() != null)
            {
                TempData.WriteAlertMessage(
                    messageText: cartService.Errors.FirstOrDefault().ErrorMessage);
                goto exit_point;
            }
            TempData.WriteAlertMessage(
                messageText: "the book has been successfully added to the cart",
                messageType: ViewAlertMessageType.Success);

        exit_point:
            return RedirectToAction(
                actionName: nameof(this.Index),
                controllerName: "Home");
        }

        public IActionResult Index()
        {
            return View(model: cartService.Lines);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(Guid id)
        {
            cartService.Remove(bookId: id);
            if (cartService.HasErrors
                && cartService.Errors.FirstOrDefault() != null)
            {
                TempData.WriteAlertMessage(
                    messageText: cartService.Errors.FirstOrDefault().ErrorMessage);
            }
            return RedirectToAction(
                actionName: nameof(this.Index),
                controllerName: "Cart");
        }

        [HttpGet]
        public ViewResult Change(Guid id)
        {
            ViewBag.EditId = id;
            return View(
                viewName: "Index",
                model: cartService.Lines);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Change(CartLineChangeQuantityViewModel changesDto)
        {
            if (!ModelState.IsValid)
            {
                goto error_exit;
            }
            cartService.SetQuantity(
                bookId: changesDto.BookId,
                quantity: changesDto.Quantity);
            if (cartService.HasErrors)
            {
                foreach (var error in cartService.Errors)
                {
                    ModelState.AddModelError(
                        key: "",
                        errorMessage: error.ErrorMessage);
                }
                goto error_exit;
            }
            return RedirectToAction(
                actionName: nameof(this.Index),
                controllerName: "Cart");

        error_exit:
            ViewBag.EditId = changesDto.BookId;
            return View(
                viewName: "Index",
                model: cartService.Lines);
        }
    }
}
