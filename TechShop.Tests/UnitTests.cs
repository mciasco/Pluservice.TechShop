using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using TechShop.Domain;
using TechShop.WS.Commons;
using Xunit;

namespace TechShop.Tests
{
    public class UnitTests : IDisposable
    {
        private readonly Mock<IProductsRepository> _mockRepo;
        private readonly IProductsRetriever _localProductsRetriever;

        public UnitTests()
        {
            // crea un mock del repository da cui prelevare i prodotti
            _mockRepo = new Mock<IProductsRepository>();
            var testCategoryA = new Categoria(10, "TestCategoryA");
            var testProducts = new[]
            {
                new Prodotto(1, "Prod1", testCategoryA), 
                new Prodotto(2, "Prod2", testCategoryA),
            };
            _mockRepo
                .Setup(r => r.GetWhere(It.IsAny<Expression<Func<Prodotto, bool>>>()))
                .Returns(Task.FromResult(testProducts.AsEnumerable()));

            _localProductsRetriever = new LocalStoreProductsRetriever(testCategoryA, _mockRepo.Object);
        }


        [Fact]
        public void Test_get_all_products_from_local_store()
        {
            var foundProducts = _localProductsRetriever.RetrieveProductsByCategory().Result;
            Assert.Equal(2, foundProducts.Count());
        }


        public void Dispose()
        {
            
        }
    }
}
