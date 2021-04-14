using CoffeDelivery.Data;
using CoffeDelivery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeDelivery.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDbClient _client;
        private string[] roles = { "Admin", "Cooker" };
        public HomeController(ILogger<HomeController> logger, IDbClient client)
        {
            _logger = logger;
            _client = client;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CreateDishForm()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDish(Dish dish)
        {
            await _client.CreateDishAsync(dish);
            return RedirectToAction("DishesList");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDishForm(int id)
        {
            Dish dish = await _client.GetDishAsync(id);
            return View(dish);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDish(Dish dish)
        {
            await _client.DeleteDishAsync(dish.Id);
            return RedirectToAction("DishesList");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDishForm(int id)
        {
            Dish dish = await _client.GetDishAsync(id);
            return View(dish);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDish(Dish dish)
        {
            await _client.UpdateDishAsync(dish);
            return RedirectToAction("DishesList");
        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOrderForm(int id)
        {

            ViewBag.Dish = await _client.GetDishAsync(id);
            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOrder(Order order)
        {
             
            order.UserId = User.Identity.Name;
            order.DateTimeOrderPlaced = DateTime.Now;
            await _client.CreateOrderAsync(order);
            return RedirectToAction("DishesList");
        }
        [Authorize]
        public async Task<IActionResult> DishesList()
        {
            List<Dish> dishes = await _client.GetDishesAsync();
            return View(dishes);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OrdersList()
        {
            List<Order> orders = await _client.GetOrdersAsync();
            return View(orders);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OrdersForCookingAsync()
        {
            List<Order> orders = await _client.GetOrdersForCookingAsync();
            orders.OrderBy(o => o.CourierId);
            return View(orders);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OrdersForDeliveryAsync()
        {
            List<Order> orders = await _client.GetOrdersForDeliveryAsync();
            return View(orders);
        }
        [Authorize(Roles = "Cooker")]
        public async Task<IActionResult> StartCooking(int id)
        {
            Order order = await _client.GetOrderAsync(id);
            order.CookerId = User.Identity.Name;
            var result = _client.UpdateOrderAsync(order);
            return RedirectToAction("OrdersForCooking");
        }
        [Authorize(Roles = "Cooker")]
        public async Task<IActionResult> FinishCooking(int id)
        {
            Order order = await _client.GetOrderAsync(id);
            order.DateTimeOrderCooked = DateTime.Now;
            var result = _client.UpdateOrderAsync(order);
            return RedirectToAction("OrdersForCooking");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MyOrders()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<Order> orders = await _client.GetOrdersByUserAsync(User.Identity.Name);
                 
                return View(orders);
            }
            else
            {
                return View(new List<Order>());
            }
          
                               

           
        }





        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
