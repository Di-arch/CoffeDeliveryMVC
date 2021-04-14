using CoffeDelivery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeDelivery.Data
{
    public interface  IDbClient
    {
        Task<Dish> GetDishAsync(int id);
        Task<List<Dish>> GetDishesAsync();
        Task<int> CreateDishAsync(Dish dish);
        Task<bool> UpdateDishAsync(Dish dish);
        Task<bool> DeleteDishAsync(int id);
        Task<Order> GetOrderAsync(int id);
        Task<List<Order>> GetOrdersAsync();
        Task<List<Order>> GetOrdersByUserAsync(string user);
        Task<List<Order>> GetOrdersAsync(int page, int rowsPerPage);
        Task<List<Order>> GetOrdersForCookingAsync();
        Task<List<Order>> GetOutDatedOrdersForCookingAsync();
        Task<List<Order>> GetOrdersForDeliveryAsync();
        Task<List<Order>> GetOutdatedOrdersForDeliveryAsync();
        Task<List<Order>> GetOrdersByCookerAsync(string cookecrId);
        Task<List<Order>> GetOrdersByCourierAsync(string courierId);
        Task<int> CreateOrderAsync(Order order);
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int id);
    }
}
