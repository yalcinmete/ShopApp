using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.ViewComponents
{
    public class CategoryListViewComponent : ViewComponent
    {
       private ICategoryService _categoryService;

        public CategoryListViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            //return View(_categoryService.GetAll());  
            return View(new CategoryListViewModel()
            {
                SelectedCategory = RouteData.Values["category"]?.ToString(), //category değeri null gelirse stringe çeviremez hata verir bu nedenle soru işareti koyduk(null değilse)
                Categories = _categoryService.GetAll()
            });  
        }
    }
}
