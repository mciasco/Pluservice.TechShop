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
using System.Web;
using Microsoft.AspNetCore.Authentication;
using TechShop.Data.InMemoryDb;
using TechShop.Domain;
using TechShop.WS.Commons;
using WSF.Authentication;
using WSF.Handlers;

namespace WSF
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services
                .AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            // registra un semplice sistema di controllo delle credenziali utente ricevute
            // che in questo caso convalida qualunque username/password
            // Un sistema più evoluto si appoggerebbe ad un repository di utenti
            services.AddScoped<IUserAuthenticatorService, EveryUserAuthenticatorService>();

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

            // registra un servizio per la gestione custom degli errori della pipeline
            services.AddTransient<IErrorHandlerService, CustomErrorHandlerService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // middleware custom per la gestione degli errori
            // eventualmente combinabile (decorator) con un handler di log che effettua il log con il servizo ILogger
            // delle eccezioni gestite da questo middleware
            app.UseErrorHandlerMiddleware();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("Default", "{controller=Info}/{action=Get}/{id?}");
            });
        }
    }
}
