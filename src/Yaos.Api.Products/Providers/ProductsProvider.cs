using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Yaos.Api.Products.Db;
using Yaos.Api.Products.Interfaces;
using Yaos.Api.Products.Models;

namespace Yaos.Api.Products.Providers
{
    public class ProductsProvider : IProductsProvider
    {
        private readonly ProductsDbContext dbContext;
        private readonly IMapper mapper;

        public ProductsProvider(ProductsDbContext dbContext, ILogger<ProductsProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            Logger = logger;
            this.mapper = mapper;
            SeedData();
        }

        private void SeedData()
        {
            if (dbContext.Products.Any())
                return;

            dbContext.Products.Add(new Db.Product() { Id = 1, Name = "Keyboard", Price = 20, Inventory = 100 });
            dbContext.Products.Add(new Db.Product() { Id = 2, Name = "Mouse", Price = 5, Inventory = 200 });
            dbContext.Products.Add(new Db.Product() { Id = 3, Name = "Monitor", Price = 150, Inventory = 120 });
            dbContext.Products.Add(new Db.Product() { Id = 4, Name = "CPU", Price = 200, Inventory = 150 });
            dbContext.SaveChanges();
        }

        public ILogger<ProductsProvider> Logger { get; }

        public async Task<(bool IsSuccess, IEnumerable<Models.Product>? Products, string? ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var products = await dbContext.Products.ToListAsync();
                if(products != null && products.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Product>, IEnumerable<Models.Product>>(products);
                    return (true, result, null);
                }

                return (false, null, "NotFound");
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.Product? Product, string? ErrorMessage)> GetProductAsync(int id)
        {
            try
            {
                var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product != null)
                {
                    var result = mapper.Map<Db.Product,Models.Product>(product);
                    return (true, result, null);
                }

                return (false, null, "NotFound");
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
