﻿using FreeCourse.Web.Models.Order;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public OrderController(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Checkout()
        {
            var basket = await _basketService.Get();
            
            ViewBag.basket = basket;
            return View(new CheckoutInfo());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutInfo checkoutInfo)
        {
            //I.way sync communication
            var orderStatus = await _orderService.CreateOrder(checkoutInfo);

            //II.way async communication
            if (!orderStatus.IsSuccessful)
            {
                var basket = await _basketService.Get();
                ViewBag.basket = basket;
                ViewBag.error = orderStatus.Error;
                return View();
            }
            return RedirectToAction(nameof(SuccessfulCheckout), new { orderId = orderStatus.OrderId});
        }

        public IActionResult SuccessfulCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }
    }
}
