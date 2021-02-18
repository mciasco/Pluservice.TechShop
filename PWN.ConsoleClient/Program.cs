using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TechShop.Domain;

namespace PWN.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TECH SHOP");
            Console.WriteLine("Premere INVIO per mostrare i prodotti disponibili:");
            Console.ReadLine();

            MainAsync().Wait();
            
            Console.WriteLine("------------------------------");
            Console.WriteLine("Premere INVIO per uscire");
            Console.ReadLine();

        }

        static async Task MainAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                HttpResponseMessage responseProdotti = await client.GetAsync("catalogo");
                if (responseProdotti.IsSuccessStatusCode)
                {
                    var products = await responseProdotti.Content.ReadAsAsync<IEnumerable<Prodotto>>();
                    foreach (var product in products)
                        Console.WriteLine($"[{product.Id}] : {product.Description} - {product.ParentCatergory.Description}");
                }
                else
                {
                    Console.WriteLine("Errore. Nessun prodotto restituito");
                }
            }
        }
    }
}
