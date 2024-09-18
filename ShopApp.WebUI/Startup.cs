﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ShopApp.Business.Abstract;
using ShopApp.Business.Concrete;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EfCore;
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
        public void ConfigureServices(IServiceCollection services)
        {
            //IProductService çağrıldığında ProductManager çağrılacak,  ProductManager de içindeki IProductDal'ı çağırdığında MemoryProductDal çağrılmış olucak.
            //services.AddScoped<IProductDal, MemoryProductDal>();  //IProductDal teknolojisi değişirse ,
            services.AddScoped<IProductDal, EfCoreProductDal>();  //IProductDal teknolojisi değişirse ,
            services.AddScoped<ICategoryDal, EfCoreCategoryDal>();  //IProductDal teknolojisi değişirse ,
            services.AddScoped<IProductService, ProductManager>(); //IProductService business katmanı değişirse,
            services.AddScoped<ICategoryService, CategoryManager>(); //IProductService business katmanı değişirse,

            services.AddMvc()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "adminProducts",
                    template: "admin/products",
                    defaults: new { controller = "Admin", action = "Index" });

                routes.MapRoute(
                   name: "adminProductsEdit",
                   template: "admin/products/{id?}",
                   defaults: new { controller = "Admin", action = "Edit" });

                routes.MapRoute(
                    name: "products",
                    template: "products/{category?}",
                    defaults: new { controller = "Shop", action = "List" } //Kullanıcı /products giderse defaults çalışır.
                    );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
