﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopApp.Business.Abstract;
using ShopApp.Business.Concrete;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EfCore;
using ShopApp.WebUI.EmailServices;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationIdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            services.AddIdentity<ApplicationUser,IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // password

                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                // options.User.AllowedUserNameCharacters = "";
                options.User.RequireUniqueEmail = true;

                //kullanıcı mail confirm şimdilik false cekiyoruz daha sonradan kullanıcıya mail gönderip hesabını onaylamasını söyleyeceğiz.
                //options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            //cookie ayarları
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".ShopApp.Security.Cookie",
                    SameSite = SameSiteMode.Strict //Session sadece bizim tarayıcımızdan servera taşınır.
                 };

            });


            //IProductService çağrıldığında ProductManager çağrılacak,  ProductManager de içindeki IProductDal'ı çağırdığında MemoryProductDal çağrılmış olucak.
            //services.AddScoped<IProductDal, MemoryProductDal>();  //IProductDal teknolojisi değişirse ,
            services.AddScoped<IProductDal, EfCoreProductDal>();  //IProductDal teknolojisi değişirse ,
            services.AddScoped<ICategoryDal, EfCoreCategoryDal>();  //IProductDal teknolojisi değişirse ,
            services.AddScoped<ICartDal, EfCoreCartDal>();
            services.AddScoped<IOrderDal, EfCoreOrderDal>();

            services.AddScoped<IProductService, ProductManager>(); //IProductService business katmanı değişirse,
            services.AddScoped<ICategoryService, CategoryManager>(); //IProductService business katmanı değişirse,
            services.AddScoped<ICartService, CartManager>(); 
            services.AddScoped<IOrderService, OrderManager>();

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SeedDatabase.Seed();
            }

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

            app.UseStaticFiles(); //wwwroot'u dışarıya açar.
            app.CustomStaticFiles(); //node_modules klasörünü dışarı açtık.

            //app.UseMvcWithDefaultRoute(); //controller action ve id parametresi isteğe bağlı.

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "adminProducts",
                    template: "admin/products",
                    defaults: new { controller = "Admin", action = "ProductList" });


                routes.MapRoute(
                    name: "cart",
                    template: "cart",
                    defaults: new { controller = "Cart", action = "Index" });


                routes.MapRoute(
                    name: "orders",
                    template: "orders",
                    defaults: new { controller = "Cart", action = "GetOrders" });

                routes.MapRoute(
                    name: "checkout",
                    template: "checkout",
                    defaults: new { controller = "Cart", action = "Checkout" });



                routes.MapRoute(
                   name: "adminProductsEdit",
                   template: "admin/products/{id?}",
                   defaults: new { controller = "Admin", action = "EditProduct" });

                routes.MapRoute(
                    name: "products",
                    template: "products/{category?}",
                    defaults: new { controller = "Shop", action = "List" }); //Kullanıcı /products giderse defaults çalışır.


                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            SeedIdentity.Seed(userManager, roleManager, Configuration).Wait();
        }
    }
}
