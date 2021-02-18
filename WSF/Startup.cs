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
using TechShop.Data.InMemoryDb;
using TechShop.Domain;
using TechShop.WS.Commons;

namespace WSF
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

            services.AddControllers();

            // registra repository in memory per le categorie
            services.AddSingleton<ICategoriesRepository, InMemoryCategoriesRepository>(sp =>
            {
                var categoriesRepo = new InMemoryCategoriesRepository();
                categoriesRepo.Add(new Categoria(20, "Game")); // categoria di prodotti gestiti dal fornitore
                return categoriesRepo;
            });
            
            // registra repository in memory per i prodotti
            services.AddSingleton<IProductsRepository, InMemoryProductsRepository>(sp =>
            {
                // aggiunge alcuni prodotti del fornitore
                var productsRepo = new InMemoryProductsRepository();
                var categoriesRepo = sp.GetService<ICategoriesRepository>();
                var networkingCategory = categoriesRepo.GetByDescription("Game").Result;
                productsRepo.Add(new Prodotto(101, "Playstation", networkingCategory));
                productsRepo.Add(new Prodotto(102, "XBox", networkingCategory));
                productsRepo.Add(new Prodotto(103, "Nintendo", networkingCategory));
                return productsRepo;
            });

            services.AddSingleton<IProductsRetrieverFactory, ProductsRetrieverCustomFactory>(sp =>
            {
                // registra una factory per il retriever dei prodotti prelevandoli solo localmente
                return new ProductsRetrieverCustomFactory(category =>
                    new LocalStoreProductsRetriever(category, sp.GetService<IProductsRepository>()));
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
