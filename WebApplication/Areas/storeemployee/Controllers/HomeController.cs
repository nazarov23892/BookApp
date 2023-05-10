using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApp.BLL.Services.OrderProcessing;
using WebApplication.Infrastructure;
using WebApplication.Models;

namespace WebApplication.Areas.storeemployee.Controllers
{
    [Area(areaName: "storeemployee")]
    public class HomeController : Controller
    {
        private readonly IOrderProcessingDbAccess orderProcessingDbAccess;
        private readonly IOrderProcessingService orderProcessingService;

        public HomeController(
            IOrderProcessingDbAccess orderProcessingDbAccess,
            IOrderProcessingService orderProcessingService)
        {
            this.orderProcessingDbAccess = orderProcessingDbAccess;
            this.orderProcessingService = orderProcessingService;
        }

        public IActionResult Index()
        {
            var newOrders = orderProcessingDbAccess.GetOrders();
            return View(model: newOrders);
        }

        public IActionResult Details(int id)
        {
            var orderDto = orderProcessingService.GetOrder(orderId: id);
            if (orderDto == null)
            {
                return NotFound();
            }
            return View(model: orderDto);
        }

        [HttpPost]
        public IActionResult GotoAssembling(int id)
        {
            orderProcessingService.SetOrderStatusToAssembling(orderId: id);
            if (orderProcessingService.HasErrors)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var error in orderProcessingService.Errors)
                {
                    stringBuilder.AppendLine(error.ErrorMessage);
                }
                TempData.WriteAlertMessage(
                    messageText: stringBuilder.ToString(),
                    messageType: ViewAlertMessageType.Danger);
                goto error_exit;
            }
            return RedirectToAction(
                actionName: nameof(this.Assembling),
                controllerName: "Home",
                routeValues: new { id = id, area = "storeemployee" });

        error_exit:
            return RedirectToAction(
                actionName: nameof(this.Details),
                controllerName: "Home",
                routeValues: new { id = id, area = "storeemployee" });
        }

        [HttpGet]
        public IActionResult Assembling(int id)
        {
            var orderDto = orderProcessingService.GetOrderForAssembling(orderId: id);
            if (orderDto == null)
            {
                return NotFound();
            }
            return View(model: orderDto);
        }

        [HttpPost]
        public IActionResult GotoReady(OrderAssemblingCompletedDto orderAssemblingDto)
        {
            orderProcessingService.SetOrderStatusToReady(orderAssemblingDto);
            if (!orderProcessingService.HasErrors)
            {
                return RedirectToAction(
                    actionName: nameof(this.Index),
                    controllerName: "Home",
                    routeValues: new { area = "storeemployee" });
            }

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var error in orderProcessingService.Errors)
            {
                stringBuilder.AppendLine(error.ErrorMessage);
            }
            TempData.WriteAlertMessage(
                messageText: stringBuilder.ToString(),
                messageType: ViewAlertMessageType.Danger);

            return RedirectToAction(
                actionName: nameof(this.Assembling),
                controllerName: "Home",
                routeValues: new { id = orderAssemblingDto.OrderId, area = "storeemployee" });
        }

        [HttpPost]
        public IActionResult GotoCompleted(int id)
        {
            orderProcessingService.SetOrderStatusToCompleted(orderId: id);
            if (orderProcessingService.HasErrors)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var error in orderProcessingService.Errors)
                {
                    stringBuilder.AppendLine(error.ErrorMessage);
                }
                TempData.WriteAlertMessage(
                    messageText: stringBuilder.ToString(),
                    messageType: ViewAlertMessageType.Danger);
            }
            return RedirectToAction(
                actionName: nameof(this.Details),
                controllerName: "Home",
                routeValues: new { id = id, area = "storeemployee" });
        }
    }
}
