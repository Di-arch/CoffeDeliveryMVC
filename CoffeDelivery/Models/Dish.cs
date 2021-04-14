using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeDelivery.Data
{
    public class Dish
    {
        public int Id { get; set; }

        [Display(Name="Наименование")]
        public string Name { get; set; }

        [Display(Name = "Цена")]
        public int Price { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Изображение")]
        public string PictureUrl { get; set; }
    }
}
