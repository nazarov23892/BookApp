using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApp.BLL.Services.OrderProcessing;

namespace WebApplication.Areas.storeemployee.Controllers
{
    [Area(areaName: "storeemployee")]
    public class HomeController : Controller
    {
        private readonly IOrderProcessingDbAccess orderProcessingDbAccess;

        public HomeController(IOrderProcessingDbAccess orderProcessingDbAccess)
        {
            this.orderProcessingDbAccess = orderProcessingDbAccess;
        }

        public IActionResult Index()
        {
            var newOrders = orderProcessingDbAccess.GetOrders();
            return View(model: newOrders);
        }

        public IActionResult Details(int id)
        {
            var orderDto = orderProcessingDbAccess.GetOrder(orderId: id);
            if (orderDto == null)
            {
                return NotFound();
            }
            return View(model: orderDto);
        }
    }
}
