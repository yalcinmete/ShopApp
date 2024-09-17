using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    //public class EfCoreProductDal : IProductDal
    public class EfCoreProductDal : EfCoreGenericRepository<Product, ShopContext>, IProductDal
    {

        public Product GetProductDetails(int id)
        {
            using (var context = new ShopContext())
            {
                //Producttan Categorylere productCategories üzerinden ulasıyoruz.
                return context.Products
                    .Where(i=>i.Id ==id)
                    .Include(i=>i.ProductCategories)
                    .ThenInclude(i=>i.Category)
                    .FirstOrDefault();

            }
        }

        public List<Product> GetProductsByCategory(string category,int page,int pageSize)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.AsQueryable();

                //category bilgisi gönderilirse;
                if (!string.IsNullOrEmpty(category))
                {
                    products = products
                                .Include(i => i.ProductCategories)
                                .ThenInclude(i => i.Category)
                                .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
                }

                //return products.ToList();
                return products.Skip((page-1)*pageSize).Take(pageSize).ToList();

            }
        }
    }
}
