using Yaos.Api.Search.Interfaces;

namespace Yaos.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService ordersService;
        private readonly IProductsService productsService;
        private readonly ICustomersService customersService;

        public SearchService(IOrdersService ordersService, IProductsService productsService, ICustomersService customersService)
        {
            this.ordersService = ordersService;
            this.productsService = productsService;
            this.customersService = customersService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var customersResult = await customersService.GetCustomerAsync(customerId);
            var ordersResult = await ordersService.GetOrdersAsync(customerId);
            var productsResult = await productsService.GetProductsAsync();

            if(ordersResult.IsSuccess)
            {
                foreach (var order in ordersResult.Orders)
                {
                    foreach (var item in order.Items)
                    {

                        item.ProductName = !productsResult.IsSuccess ?
                            "Product Information is not available" :
                            productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name;
                    }
                }
                var result = new
                {
                    Customers = customersResult.IsSuccess ? customersResult.Customers : new Models.Customer() { Name = "Information Unavailable", Address = "Information Unavailable" },
                    Orders = ordersResult.Orders
                };
                return (true, result);
            }

            return (false, null);
        }
    }
}
