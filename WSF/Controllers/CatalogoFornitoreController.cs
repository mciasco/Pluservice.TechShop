using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechShop.Domain;
using TechShop.WS.Commons;

namespace WSF.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CatalogoFornitoreController : ControllerBase
    {
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IProductsRetrieverFactory _productsRetrieverFactory;

        public CatalogoFornitoreController(ICategoriesRepository categoriesRepository, IProductsRetrieverFactory productsRetrieverFactory)
        {
            _categoriesRepository = categoriesRepository;
            _productsRetrieverFactory = productsRetrieverFactory;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prodotto>>> Get(string categoryId = "")
        {
            var allproducts = new List<Prodotto>();

            var allCategories = await (string.IsNullOrEmpty(categoryId)
                ? _categoriesRepository.GetAll()
                : _categoriesRepository.GetWhere(c => c.Id.ToString().Equals(categoryId)));

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