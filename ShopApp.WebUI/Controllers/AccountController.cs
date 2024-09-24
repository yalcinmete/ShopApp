using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
    [AutoValidateAntiforgeryToken] //Actionların başına [ValidateAntiForgeryToken] yazmak yerine controllerin başına bu şekilde token kontrolünü ekleyebilirsin.Böylece GET metodu haric Post metotlarının hepsi token kontrolünü yapar.
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // generate token
                // send email

                return RedirectToAction("account", "login");
            }

            //ModelState'de herhangi bir property ile ilişkilendirmiyoruz "" boş veriyoruz ilk parametreyi Genel olarak ekranda hata alınsın.   
            ModelState.AddModelError("", "Bilinmeyen hata oluştu lütfen tekrar deneyiniz.");
            return View(model);
        }

        public IActionResult Login(string ReturnUrl = null)
        {

            return View(new LoginModel()
            {
                 ReturnUrl = ReturnUrl
            });
        }


        [HttpPost]
        
        public async Task<IActionResult> Login(LoginModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //var user = await _userManager.FindByNameAsync(model.Username);
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                //ModelState.AddModelError("", "Bu kullanıcı ile daha önce hesap oluşturulmamış.");
                ModelState.AddModelError("", "Bu mail ile daha önce hesap oluşturulmamış.");
                return View(model);
            }

            //3.parametre true : cookie yaşam süresi, tarayıcıyı kapatmış olsan bile (startupda 60dk verdik) cookie devam edicek.
            //var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, false);
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl ?? "~/");
            }

            //ModelState.AddModelError("", "Kullanıcı adı ve ya parola yanlış");
            ModelState.AddModelError("", "Mail ya da parola yanlış");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }

    }
}
