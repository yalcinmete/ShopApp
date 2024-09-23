using Microsoft.AspNetCore.Identity;

namespace ShopApp.WebUI.Identity
{
    //IdentityUser Class'ı da kullanılabilir ama biz başka propertyler de tanımlamak istiyoruz.Bu nedenle Yeni bir class olusturup IdentityUser'dan miras aldık.
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
