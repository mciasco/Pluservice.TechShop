using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShop.Domain;

namespace WSN.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ApiInfo>> Get()
        {
            return await Task.FromResult(Ok(new ApiInfo("WSN")));
        }
    }

}
