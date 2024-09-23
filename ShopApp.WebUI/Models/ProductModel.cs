using ShopApp.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUI.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        //[Required]
        //[StringLength(60,MinimumLength =10,ErrorMessage ="Ürün ismi minimum 10 karakter en fazla 60 karakter olmalı.")]
        public string Name { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required(ErrorMessage ="Fiyat Belirtiniz")]
        [Range(1,10000)]
        //Decimal değeri ? koymazsan direkt 0 olarak atama yapar.Requied anlamsız olur.Bu nedenle ? koyalım.
        public decimal? Price { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 20, ErrorMessage = "Ürün açıklaması minimum 20 karakter en fazla 100 karakter olmalı.")]
        public string Description { get; set; }

        //Bu product için seçilen categoriler anlamında SelectedCategories isimlendirmesi yapalım.
        public List<Category> SelectedCategories { get; set; }
    }
}
