using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TechShop.Domain;
using TechShop.WS.Commons;

namespace WSN.Controllers
{
    
    
    [Route("[controller]")]
    [ApiController]
    public class CatalogoController : ControllerBase
    {
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IProductsRetrieverFactory _productsRetrieverFactory;

        public CatalogoController(ICategoriesRepository categoriesRepository, IProductsRepository productsRepository, IProductsRetrieverFactory productsRetrieverFactory)
        {
            _categoriesRepository = categoriesRepository;
            _productsRepository = productsRepository;
            _productsRetrieverFactory = productsRetrieverFactory;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prodotto>>> Get()
        {
            var allproducts = new List<Prodotto>();
            
            var allCategories = await _categoriesRepository.GetAll();
            foreach (var category in allCategories)
            {
                var productsRetriever = _productsRetrieverFactory.CreateProductsRetrieverFor(category);
                var childrenProducts = await productsRetriever.RetrieveProductsByCategory();
                allproducts.AddRange(childrenProducts);
            }

            return await Task.FromResult(
                new ActionResult<IEnumerable<Prodotto>>(
                    Ok(allproducts)));

        }
    }

    



}
