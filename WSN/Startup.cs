using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShop.Contracts.Data;
using TechShop.Data.InMemoryDb;
using TechShop.Domain;

namespace WSN
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddSingleton<ICategoriesRepository, InMemoryCategoriesRepository>(sp =>
            {
                var categoriesRepo = new InMemoryCategoriesRepository();
                categoriesRepo.Add(new Categoria(100, "Networking"));
                categoriesRepo.Add(new Categoria(200, "Game"));
                return categoriesRepo;
            });

            services.AddSingleton<IProductsRepository, InMemoryProductsRepository>(sp =>
            {
                var productsRepo = new InMemoryProductsRepository();
                var categoriesRepo = sp.GetService<ICategoriesRepository>();
                var networkingCategory = categoriesRepo.GetByDescription("Networking").Result;
                var router = new Prodotto(1, "Router", networkingCategory);
                var accessPoint = new Prodotto(2, "Access point", networkingCategory);
                var switchDevice = new Prodotto(3, "Switch", networkingCategory);
                productsRepo.Add(router);
                productsRepo.Add(accessPoint);
                productsRepo.Add(switchDevice);
                return productsRepo;
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("Default", "{controller=Info}/{action=Get}/{id?}");
            });
        }
    }

}
