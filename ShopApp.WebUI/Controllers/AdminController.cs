﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using ShopApp.WebUI.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
    //[Authorize]
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;

        public AdminController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult ProductList()
        {
            return View(new ProductListModel()
            {
                Products = _productService.GetAll()
            });
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View(new ProductModel());
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new Product()
                {
                    Name = model.Name,
                    Price = model.Price,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl,
                };
                if ( _productService.Create(entity))
                {
                    return RedirectToAction("ProductList");
                }
                ViewBag.ErrorMessage = _productService.ErrorMessage;
                return View(model);
            }
            return View(model);           
        }

        [HttpGet]
        public IActionResult EditProduct(int? id )
        {
            if (id == null)
            {
                return NotFound();
            }

            //var entity = _productService.GetById((int)id);
            var entity = _productService.GetByIdWithCategories((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl,

                SelectedCategories = entity.ProductCategories.Select(i=>i.Category).ToList()
            };

            //Yukarıda ProductModel içinde de tüm categorileri iletebilirdik ama Modeli çok kirletmemek için ViewBag ile gönderdik. 
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductModel model, int[] categoryIds, IFormFile file)
        {
            if (ModelState.IsValid)
            {            
                var entity = _productService.GetById(model.Id);
                if (entity ==null)
                {
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Price = model.Price;
                //entity.ImageUrl = model.ImageUrl;

                if (file!=null)
                {
                    entity.ImageUrl = file.FileName;

                    //her yeni eklenen resim bir önceki ile aynı isimde ise ezme durumu var aynı isimli resim dosyasını silip yeni isimli dosyayı verir.uniq bir değer verilmeli.
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);
                    using (var stream = new FileStream(path,FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                _productService.Update(entity, categoryIds);
                return RedirectToAction("ProductList");

            }
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }


        [HttpPost]
        public IActionResult DeleteProduct(int productId) 
        {
            var entity = _productService.GetById(productId);

            if (entity!=null)
            {
                _productService.Delete(entity);

            }
            return RedirectToAction("ProductList");

        }

        public IActionResult CategoryList()
        {
            return View(new CategoryListModel
            {
                Categories = _categoryService.GetAll()
            });
        }


        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {
            var entity = new Category()
            {
                Name = model.Name,
            };

            _categoryService.Create(entity);
            return RedirectToAction("CategoryList");
        }

        [HttpGet]
        public IActionResult EditCategory(int id )
        {
            //var entity = _categoryService.GetById(id);
            var entity = _categoryService.GetByWihProducts(id);
            return View(new CategoryModel()
            {
                Id= entity.Id,
                Name =entity.Name,
                Products = entity.ProductCategories.Select(p=>p.Product).ToList()
            });

            
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryModel model)
        {
            var entity = _categoryService.GetById(model.Id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Name = model.Name;
            _categoryService.Update(entity);

            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);

            if (entity != null)
            {
                _categoryService.Delete(entity);

            }
            return RedirectToAction("CategoryList");

        }

        [HttpPost]
        public IActionResult DeleteFromCategory(int categoryId, int productId)
        {
            //Databaseden ürünü silmiyoruz. ProductCategory tablosundan ilişkili categoryId productId satırını siliyoruz.
            _categoryService.DeleteFromCategory(categoryId, productId);

            //işlem tamamlandıktan sonra bizi aynı sayfaya (categoryıd ile gitmiş olduğumuz sayfaya) geri getirsin.
            return Redirect("/admin/editcategory/"+ categoryId);
        }
    }
}
