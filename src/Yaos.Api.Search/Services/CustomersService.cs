using System.Text.Json;
using Yaos.Api.Search.Interfaces;
using Yaos.Api.Search.Models;

namespace Yaos.Api.Search.Services
{
    public class CustomersService : ICustomersService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<CustomersService> logger;

        public CustomersService(IHttpClientFactory httpClientFactory, ILogger<CustomersService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }
        public async Task<(bool IsSuccess, Customer? Customers, string? ErrorMessage)> GetCustomerAsync(int customerId)
        {
            try
            {
                var client = httpClientFactory.CreateClient(HttpClientConstants.Customers);
                var response = await client.GetAsync($"api/customers/{customerId}");

                if(response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<Customer>(content, options);
                    return (true, result, String.Empty);
                }

                return (false, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
