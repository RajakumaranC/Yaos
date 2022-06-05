using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Yaos.Api.Search.Models;
using Yaos.Api.Search.Services;

namespace Yaos.Api.Search.Tests
{
    public class OrdersServiceTest
    {
        private OrdersService ordersServiceValidId;

        public OrdersServiceTest()
        {
            
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"[{ ""Id"": 1, ""OrderDate"": ""2022-01-19T07:22Z"", ""Total"": 100, ""Items"": [{""Id"": 1, ""ProductId"": 1, ""ProductName"": ""keyboard"", ""Quantity"":10, ""UnitPrice"":100}]}]"),
            };

            handlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response);

            var httpClient = new HttpClient(handlerMock.Object);
            httpClient.BaseAddress = new Uri("http://baseaddress");

            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var httpClientfactory = mockFactory.Object;

            ordersServiceValidId = new OrdersService(httpClientfactory, null);

        }

        [Fact]
        public async Task GetOrdersReturnOrdersAsync()
        {
            var orders = await ordersServiceValidId.GetOrdersAsync(1);

            Assert.True(orders.IsSuccess);
            Assert.True(orders.Orders.Any());
            Assert.True(orders.Orders.First().Id == 1);
            Assert.True(orders.Orders.First().Items.Any());
            Assert.True(orders.Orders.First().Items.First().Id == 1);
            Assert.True(orders.ErrorMessage == String.Empty);
        }
    }
}
