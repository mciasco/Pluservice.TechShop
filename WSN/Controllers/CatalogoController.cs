using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TechShop.Domain;

namespace WSN.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CatalogoController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prodotto>>> Get()
        {
            var networkingCategory = new Categoria(1, "Networking");

            var prodottiNetworking = new List<Prodotto>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44305/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage responseProdotti = await client.GetAsync("catalogofornitore");
                
                if (responseProdotti.IsSuccessStatusCode)
                {
                    var prodotti = await responseProdotti.Content.ReadAsAsync<IEnumerable<Prodotto>>();
                    prodottiNetworking.AddRange(prodotti);
                }
            }
            
            prodottiNetworking.AddRange(new[] { new Prodotto(1, "Router", networkingCategory), new Prodotto(2, "Range extender", networkingCategory) });

            return await Task.FromResult(
                new ActionResult<IEnumerable<Prodotto>>(
                    Ok(prodottiNetworking)));
        }
    }

    



}
