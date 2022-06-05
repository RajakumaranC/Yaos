using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Yaos.Api.Search.Services;

namespace Yaos.Api.Search.Tests
{
    public class ProductsServiceTest
    {
        private ProductsService ProductsServiceValidId;

        public ProductsServiceTest()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"[{ ""Id"": 1, ""Name"": ""Keyboard""},{ ""Id"": 2, ""Name"": ""Mouse""}]"),
            };

            handlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response);

            var httpClient = new HttpClient(handlerMock.Object);
            httpClient.BaseAddress = new Uri("http://baseaddress");

            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var httpClientfactory = mockFactory.Object;

            ProductsServiceValidId = new ProductsService(httpClientfactory, null);
        }

        [Fact]
        public async Task GetProductsReturnsProductsAsync()
        {
            var products = await ProductsServiceValidId.GetProductsAsync();

            Assert.True(products.IsSuccess);
            Assert.NotNull(products.Products);
            Assert.True(products.Products.Any());
            Assert.True(products.Products.First().Id == 1);
            Assert.True(products.ErrorMessage == String.Empty);
        }
    }
}
