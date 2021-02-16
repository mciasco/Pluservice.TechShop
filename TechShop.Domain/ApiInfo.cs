namespace TechShop.Domain
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