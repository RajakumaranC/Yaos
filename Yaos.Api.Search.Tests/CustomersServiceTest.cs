using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Yaos.Api.Search.Services;

namespace Yaos.Api.Search.Tests
{
    public class CustomersServiceTest
    {
        private CustomersService customerServiceValidId;

        public CustomersServiceTest()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{ ""Name"": ""Raj"", ""Address"": ""SampleAddress""}"),
            };

            handlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response);

            var httpClient = new HttpClient(handlerMock.Object);
            httpClient.BaseAddress = new Uri("http://baseaddress");

            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var httpClientfactory = mockFactory.Object;

            customerServiceValidId = new CustomersService(httpClientfactory, null);

        }


        [Fact]
        public async Task GetCustomerReturnsCustomerWithValidId()
        {
            var customer = await customerServiceValidId.GetCustomerAsync(1);


            Assert.True(customer.IsSuccess);
            Assert.NotNull(customer.Customers);
            Assert.True(!string.IsNullOrEmpty(customer.Customers.Name));
            Assert.True(!string.IsNullOrEmpty(customer.Customers.Address));
            Assert.True(customer.ErrorMessage == String.Empty);
        }

    }
}