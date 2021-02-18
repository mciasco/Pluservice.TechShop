using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TechShop.Domain;

namespace TechShop.WS.Commons
{
    public interface IProductsRetriever
    {
        Categoria Category { get; }

        Task<IEnumerable<Prodotto>> RetrieveProductsByCategory();
    }

    public class LocalStoreProductsRetriever : IProductsRetriever
    {
        private readonly IProductsRepository _productsRepository;

        public LocalStoreProductsRetriever(Categoria category, IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
            Category = category;
        }

        public Categoria Category { get; }

        public async Task<IEnumerable<Prodotto>> RetrieveProductsByCategory()
        {
            return await _productsRepository.GetWhere(p => p.ParentCatergory.Id == Category.Id);
        }
    }

    public class RemoteStoreProductsRetriever : IProductsRetriever
    {
        private readonly string _remoteApiBaseUrl;
        private readonly string _apiControllerName;

        public RemoteStoreProductsRetriever(string remoteApiBaseUrl, string apiControllerName, Categoria category)
        {
            Category = category;
            _remoteApiBaseUrl = remoteApiBaseUrl; 
            _apiControllerName = apiControllerName;

        }

        public Categoria Category { get; }

        public async Task<IEnumerable<Prodotto>> RetrieveProductsByCategory()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_remoteApiBaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // aggiunge l'header per la Basic Authentication con un username e password qualunque
                // lato WSF infatti è registrato un sistema di autenticazione che convalida qualunque username/password
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(Encoding.UTF8.GetBytes("admin:1234")));

                string apiUrl = $"{_apiControllerName}?categoryId={Category.Id}";
                HttpResponseMessage responseProdotti = await client.GetAsync(apiUrl);

                if (responseProdotti.IsSuccessStatusCode)
                {
                    return await responseProdotti.Content.ReadAsAsync<IEnumerable<Prodotto>>();
                }
                else
                {
                    return await Task.FromResult(Enumerable.Empty<Prodotto>());
                }
            }
        }
    }

    public interface IProductsRetrieverFactory
    {
        IProductsRetriever CreateProductsRetrieverFor(Categoria category);
    }

    public class ProductsRetrieverFactoryByCategoryId : IProductsRetrieverFactory
    {
        private readonly string _remoteApiBaseUrl;
        private readonly string _apiControllerName;
        private readonly IProductsRepository _productsRepository;
        private readonly string _categoryId;

        public ProductsRetrieverFactoryByCategoryId(
            string remoteApiBaseUrl, 
            string apiControllerName, 
            IProductsRepository productsRepository)
        {
            _remoteApiBaseUrl = remoteApiBaseUrl;
            _apiControllerName = apiControllerName;
            _productsRepository = productsRepository;
        }

        public IProductsRetriever CreateProductsRetrieverFor(Categoria category)
        {
            return category.Id <= 10
                ? (IProductsRetriever)new LocalStoreProductsRetriever(category, _productsRepository)
                : new RemoteStoreProductsRetriever(_remoteApiBaseUrl, _apiControllerName, category);
        }
    }


    public class ProductsRetrieverCustomFactory : IProductsRetrieverFactory
    {
        private readonly Func<Categoria, IProductsRetriever> _retrieverFactoryMethod;

        public ProductsRetrieverCustomFactory(Func<Categoria, IProductsRetriever> retrieverFactoryMethod)
        {
            _retrieverFactoryMethod = retrieverFactoryMethod;
        }

        public IProductsRetriever CreateProductsRetrieverFor(Categoria category)
        {
            return _retrieverFactoryMethod(category);
        }
    }
}
