using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Concrete
{
    public class CartManager : ICartService
    {
        private ICartDal _cartDal;
        public CartManager(ICartDal cartDal)
        {
            _cartDal = cartDal;
        }

        public void AddToCart(string userId, int productId, int quantity)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                //Aynı product ürününü ekliyorsa kullanıcısı cart açılmamalı mevcut cart 1 arttırılmalı.
                var index = cart.CartItem.FindIndex(i => i.ProductId == productId);
                //Eğer 0 veya 0 dan büyük bir değer var ise, Listenin herhangibir yerinde productId var.

                if (index < 0) //Bu kayıt liste içerisinde olmadığından dolayı yeniden ekleyeceğiz.
                {
                    cart.CartItem.Add(new CartItem()
                    {
                        ProductId = productId,
                        Quantity = quantity,
                        CartId = cart.Id
                    });
                }
                else
                {
                    cart.CartItem[index].Quantity += quantity;
                }

                _cartDal.Update(cart);
            }
        }

        public void ClearCart(string cardId)
        {
            _cartDal.ClearCart(cardId);
        }

        public void DeleteFromCart(string userId, int productId)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                _cartDal.DeleteFromCart(cart.Id, productId);
            }
        }

        public Cart GetCartByUserId(string userId)
        {
            return _cartDal.GetByUserId(userId);
        }

        public void InitializeCart(string userId)
        {
            _cartDal.Create(new Cart() { UserId = userId });
        }
    }
}
