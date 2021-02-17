using System;
using System.Collections.Generic;
using System.Text;

namespace TechShop.Contracts.Apis
{
    public class ApiInfo
    {
        public string ApiName { get; private set; }

        internal ApiInfo()
        {
            ApiName = "NonameApi";
        }

        public ApiInfo(string apiName)
        {
            ApiName = apiName;
        }
    }
}
