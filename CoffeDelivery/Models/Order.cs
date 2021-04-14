using CoffeDelivery.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeDelivery.Models
{
    public class Order
    {
        [Display(Name = "Код заказа")]
        public int Id { get; set; }
        [Display(Name ="Заказчик")]
        public string UserId { get; set; }
        [Display(Name = "Код блюда")]
        public int DishId { get; set; }
        [Display(Name = "Кол-во")]
        public int Quantity { get; set; }
        [Display(Name = "Адрес")]
        [Required(ErrorMessage ="Необходимо указать адрес")]
        public string Address { get; set; }
        [Display(Name = "Повар")]
        public string CookerId { get; set; }
        [Display(Name = "Курьер")]
        public string CourierId { get; set; }
        [Display(Name = "Время поступления заказа")]
        public DateTime DateTimeOrderPlaced { get; set; }
        [Display(Name = "Время приготовления заказа")]
        public DateTime DateTimeOrderCooked { get; set; }
        [Display(Name = "Время доставки заказа")]
        public DateTime DateTimeOrderDelivered { get; set; }
        public Dish Dish { get; set; }
    }
}
