using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        //Product içindeki Price fiyatı güncellenmiş ise , bu kullanıcıyı bağlamaz. Kullanıcı sepete ne attıysa onunla devam edebilmeli.Bu nedenle price alanı ekledik.
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
