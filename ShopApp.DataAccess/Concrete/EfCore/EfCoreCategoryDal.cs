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
    //public class EfCoreCategoryDal : ICategoryDal
    public class EfCoreCategoryDal : EfCoreGenericRepository<Category, ShopContext>, ICategoryDal
    {
        public void DeleteFromCategory(int categoryId, int productId)
        {
            using (var context = new ShopContext())
            {
                var cmd = @"delete from ProductCategory where ProductId=@p0 And CategoryId=@p1 ";
                context.Database.ExecuteSqlCommand(cmd,productId,categoryId);

                //ExecuteSqlCommand'dan dönen int değeri olan etkilenen satır sayısına göre işlemin kontrolünü yaptırabilirsin. 
            }
        }

        public Category GetByIdWithProducts(int id)
        {
            using (var context = new ShopContext())
            {
                return context.Categories
                    .Where(i => i.Id == id)
                    .Include(i=>i.ProductCategories)
                    .ThenInclude(i=>i.Product)
                    .FirstOrDefault();
            }
        }
    }
}
