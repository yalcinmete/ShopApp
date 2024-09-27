using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.DataAccess.Abstract
{
    public interface ICartDal : IRepository<Cart>
    {
        void ClearCart(string cardId);
        void DeleteFromCart(int cartId, int productId);
        Cart GetByUserId(string userId);
    }
}
