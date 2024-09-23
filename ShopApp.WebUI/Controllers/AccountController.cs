﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
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

        public IActionResult Login()
        {
            return View(new LoginModel());
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            returnUrl = returnUrl ?? "~/";

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Bu kullanıcı ile daha önce hesap oluşturulmamış.");
                return View(model);
            }

            //3.parametre true : cookie yaşam süresi, tarayıcıyı kapatmış olsan bile (startupda 60dk verdik) cookie devam edicek.
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, false);

            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }

            ModelState.AddModelError("", "Kullanıcı adı ve ya parola yanlış");
            return View(model);
        }

    }
}
