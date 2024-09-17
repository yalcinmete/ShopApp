using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using ShopApp.WebUI.Models;
using System.Linq;

namespace ShopApp.WebUI.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;

        public ShopController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Productlarla birlikte categoryleri de getirmek istediğimiz için GetProductDetails adında metodumuzu include yardımıyla oluşturduk.
            //Product product = _productService.GetById((int)id);
            Product product = _productService.GetProductDetails((int)id);
            if (product == null) 
            {
                return NotFound();  
            }
            return View(new ProductDetailsModel()
            {
                Product = product,
                Categories = product.ProductCategories.Select(i => i.Category).ToList()
                //Buradaki select ile database'e sorgu atılmıyor.product nesnesi içinde category bilgisi var select linq yardımıyla categories'in içine atıyoruz. Select burada productCategories içinde dönüyor.
            });
        }

        // /products/telefon?page=2
        public IActionResult List(string category, int page = 1)
        {
            const int pageSize = 3;

            return View(new ProductListModel()
            {
                //Products = _productService.GetAll()
                Products = _productService.GetProductsByCategory(category,page,pageSize)
            });
        }
    }
}
