using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechShop.Domain;

namespace WSF.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CatalogoFornitoreController : ControllerBase
    {
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IProductsRepository _productsRepository;

        public CatalogoFornitoreController(ICategoriesRepository categoriesRepository, IProductsRepository productsRepository)
        {
            _categoriesRepository = categoriesRepository;
            _productsRepository = productsRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prodotto>>> Get()
        {
            
            var products = new List<Prodotto>();
            var allCategories = await _categoriesRepository.GetAll();
            foreach (var category in allCategories)
            {
                var childrenProducts = await _productsRepository.GetWhere(p => p.ParentCatergory.Id == category.Id);
                products.AddRange(childrenProducts);
            }

            return await Task.FromResult(
                new ActionResult<IEnumerable<Prodotto>>(
                    Ok(products)));
        }
    }
}