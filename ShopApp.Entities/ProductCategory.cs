using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Entities
{
    public class ProductCategory
    {
        //Product ve Category arasında çoka çok ilişki kurduğumuz için buradaki ID'ye ihtiyacımız yok ama ProductCategory classının databasede tablo olarak tanımlanabilmesi için CategoryId ve ProductId birincil anahtar olarak tanımlanması gerekiyor ayrıca 1 2  1 2  iki aynı satır da gelmemeli. Bunu fluentApi yardımıyla tanımlıyoruz.
        //public int Id { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
