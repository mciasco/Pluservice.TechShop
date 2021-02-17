using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechShop.Contracts.Apis;
using TechShop.Domain;

namespace WSF.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ApiInfo>> Get()
        {
            return await Task.FromResult(Ok(new ApiInfo("WSF")));
        }
    }
}
