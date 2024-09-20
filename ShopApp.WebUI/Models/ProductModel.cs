using ShopApp.Entities;
using System.Collections.Generic;

namespace ShopApp.WebUI.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        //Bu product için seçilen categoriler anlamında SelectedCategories isimlendirmesi yapalım.
        public List<Category> SelectedCategories { get; set; }
    }
}
