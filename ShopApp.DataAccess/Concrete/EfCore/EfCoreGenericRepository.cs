using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public class EfCoreGenericRepository<T, TContext> : IRepository<T> where T : class where TContext : DbContext, new()
    {
        public virtual void Create(T entity)
        {
            using (var context = new TContext())
            {
                context.Set<T>().Add(entity);
                context.SaveChanges();
            }
        }

        public virtual void Delete(T entity)
        {
            using (var context = new TContext())
            {
                context.Set<T>().Remove(entity);
                context.SaveChanges();
            }
        }

        //IQueryable<T> de çağırabilirsin ama implemente ettiği classta toList() i çağırmak zorundasın.Biz bu zorunlugu kaldırmak için burada IEnumerable döndürüp tolist()i burada çağırıyoruz.
        //public IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null)
        public virtual List<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            using (var context = new TContext())
            {
                return filter == null
                     ? context.Set<T>().ToList()
                     : context.Set<T>().Where(filter).ToList();
            }
        }

        public virtual T GetById(int id)
        {
            using (var context = new TContext())
            {
                return context.Set<T>().Find(id);
            }
        }

        public virtual T GetOne(Expression<Func<T, bool>> filter)
        {
            using (var context = new TContext())
            {
                return context.Set<T>().Where(filter).SingleOrDefault();
            }
        }

        //Update metodu ile Class(Model) içindeki propertler için update işlemi yapabiliyoruz ama bir alt propertyler için update burada çalışmaz.İlişkili datalarda Entry() çalışmıyor. Bu nedenle ya yeni bir update metodu oluşturacağız ya da virtual anahtar kelimesini ekleyerek bu metodu ezebileceğimizi ifade ediyoruz. 
        public virtual void Update(T entity)
        {
            using (var context = new TContext())
            {
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
    
    
}
