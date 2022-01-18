using Polly;
using Yaos.Api.Search;
using Yaos.Api.Search.Interfaces;
using Yaos.Api.Search.Services;
using Yaos.Api.Search.Extensions.ServiceCollection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Scoped Services
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<ICustomersService, CustomersService>(); 
#endregion


#region HttpClientFatory
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddHttpClient(HttpClientConstants.Orders, config =>
{
    config.BaseAddress = new Uri(builder.Configuration["Services:Orders"]);
}).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(5, _ => TimeSpan.FromMilliseconds(500)));

builder.Services.AddHttpClient(HttpClientConstants.Products, config =>
{
    config.BaseAddress = new Uri(builder.Configuration["Services:Products"]);
}).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(5, _ => TimeSpan.FromMilliseconds(500)));
builder.Services.AddHttpClient(HttpClientConstants.Customers, config =>
{
    config.BaseAddress = new Uri(builder.Configuration["Services:Customers"]);
}).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(5, _ => TimeSpan.FromMilliseconds(500))); 
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
