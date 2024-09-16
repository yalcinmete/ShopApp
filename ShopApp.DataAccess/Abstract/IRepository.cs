using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;

namespace ShopApp.DataAccess.Abstract
{
    public interface IRepository<T>
    {
        T GetById(int id);
        T GetOne(Expression<Func<T, bool>> filter);

        //IQueryable<T> dönersen en sonunda database'e işlem tamamlanması için Tolist()dönmek zorundasın.
        //IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null);

        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
