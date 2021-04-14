using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CoffeDelivery.Models;
using System.Text;
using System.Net;

namespace CoffeDelivery.Data
{
    public class DbClient:IDbClient
    {
        
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly string _baseUri;

         

        public DbClient(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
            _baseUri = $"{ _configuration["CoffeDeliveryApiData:Base"]}/api/CoffeDelivery";
        }
        private async Task<T> SendGetAsync<T>(string action)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, ($"{_baseUri}/{action}"));

            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var serializedResult = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(serializedResult);
        }


        private async Task<string> SendPostPutAsync<T>(T obj, HttpMethod method,string action)
        {
            var request = new HttpRequestMessage(method, ($"{_baseUri}/{action}"));
            var body = JsonConvert.SerializeObject(obj);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);
            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                return await response.Content.ReadAsStringAsync();
            }
            else if ((int)response.StatusCode==404)
            {
                throw new KeyNotFoundException();
            }
            else
            {
                return $"Response status code:{response.StatusCode}";
            }
        }
        private async Task<bool> SendDeleteAsync(string action, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, ($"{_baseUri}/{action}/{id}"));
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var serializedResult = await response.Content.ReadAsStringAsync();
            return bool.Parse(serializedResult);
        }

        #region--------------------Dish-------------------------------------
        public async Task<Dish> GetDishAsync(int id)
        {
            return await SendGetAsync<Dish>($"GetDish/{id}");
             
        }
        public async Task< List<Dish>> GetDishesAsync()
        {
            return await SendGetAsync<List<Dish>>($"GetDishes");
             
        }
        public async Task<int>CreateDishAsync(Dish dish)
        {
            try
            {
                var result = await SendPostPutAsync<Dish>(dish, HttpMethod.Post, "CreateDish");
                return int.Parse(result);
            }
            catch (Exception)
            {
                return 0;
            }

            
        }
        public async Task<bool> UpdateDishAsync(Dish dish)
        {
            try
            {
                var result = await SendPostPutAsync<Dish>(dish, HttpMethod.Put, "UpdateDish");
                return bool.Parse(result);
            }
            catch (Exception)
            {
                return false;
            }

        }
        public async Task<bool> DeleteDishAsync(int id)
        {
            return await SendDeleteAsync("DeleteDish",id);
             
        }
        #endregion--------------------Dish-------------------------------------

        #region--------------------Order-------------------------------------
        public async Task<Order> GetOrderAsync(int id)
        {
            return await SendGetAsync<Order>($"GetOrder/{id}");
            
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await SendGetAsync<List<Order>>($"GetOrders");
        }
        public async Task<List<Order>> GetOrdersByUserAsync(string user)
        {
            return await SendGetAsync<List<Order>>($"GetOrdersByUser/{user}");
        }
        public async Task<List<Order>> GetOrdersAsync(int page,int rowsPerPage)
        {
            return await SendGetAsync<List<Order>>($"GetOrders/{page}/{rowsPerPage}");
        }
        public async Task<List<Order>> GetOrdersForCookingAsync()
        {
            return await SendGetAsync<List<Order>>($"GetOrdersForCooking");
            
        }
        public async Task<List<Order>> GetOutDatedOrdersForCookingAsync()
        {
            return await SendGetAsync<List<Order>>($"GetOutDatedOrdersForCooking");
           
        }
        public async Task<List<Order>> GetOrdersForDeliveryAsync()
        {
            return await SendGetAsync<List<Order>>($"GetOrdersForDelivery");
             
        }
        public async Task<List<Order>> GetOutdatedOrdersForDeliveryAsync()
        {
            return await SendGetAsync<List<Order>>($"GetOutdatedOrderForDelivery");
           
        }
        public async Task<List<Order>> GetOrdersByCookerAsync(string cookecrId)
        {
            return await SendGetAsync<List<Order>>($"GetOrdersByCooker/{cookecrId}");
             
        }
        public async Task<List<Order>> GetOrdersByCourierAsync(string courierId)
        {
            return await SendGetAsync<List<Order>>($"GetOrderByCourier/{courierId}");
            
        }
        public async Task<int> CreateOrderAsync(Order order)
        {
            try
            {
                var result = await SendPostPutAsync<Order>(order, HttpMethod.Post, "CreateOrder");
                return int.Parse(result);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<bool> UpdateOrderAsync(Order order)
        {
            try
            {
                var result = await SendPostPutAsync<Order>(order, HttpMethod.Put, "UpdateOrder");
                return bool.Parse(result);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> DeleteOrderAsync(int id)
        {
            return await SendDeleteAsync("DeleteOrder", id);
        }
        #endregion--------------------Order-------------------------------------


    }

}
