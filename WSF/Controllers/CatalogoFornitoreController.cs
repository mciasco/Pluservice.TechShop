using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechShop.Domain;

namespace WSF.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CatalogoFornitoreController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prodotto>>> Get()
        {
            var networkingCategory = new Categoria(1, "Networking");

            return await Task.FromResult(
                new ActionResult<IEnumerable<Prodotto>>(
                    Ok(new[] { new Prodotto(11, "Ethernet cable", networkingCategory), new Prodotto(12, "Phone cable", networkingCategory) })));
        }
    }
}