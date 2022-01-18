namespace Yaos.Api.Search.Extensions.ServiceCollection
{
    public static class HttpClientsExtension
    {
        public static IServiceCollection YaosHttpClients(this IServiceCollection Services)
        {
            //Services.AddHttpClient(HttpClientConstants.Orders, config =>
            //{
            //    config.BaseAddress = new Uri(builder.Configuration["Services:Orders"]);
            //}).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(5, _ => TimeSpan.FromMilliseconds(500)));

            //Services.AddHttpClient(HttpClientConstants.Products, config =>
            //{
            //    config.BaseAddress = new Uri(builder.Configuration["Services:Products"]);
            //}).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(5, _ => TimeSpan.FromMilliseconds(500)));
            //Services.AddHttpClient(HttpClientConstants.Customers, config =>
            //{
            //    config.BaseAddress = new Uri(builder.Configuration["Services:Customers"]);
            //}).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(5, _ => TimeSpan.FromMilliseconds(500)));

            return Services;
        }
    }
}
