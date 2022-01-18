using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Yaos.Api.Products.Db;
using Yaos.Api.Products.Profiles;
using Yaos.Api.Products.Providers;

namespace Yaos.Api.Products.Tests
{
    public class ProductsServiceTest
    {
        [Fact]
        public async Task GetProductsReturnsAllProductsAsync()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductsReturnsAllProductsAsync))
                .Options;
            var dbContext = new ProductsDbContext(options);

            CreateProducts(dbContext);

            var productprofile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productprofile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var product = await productsProvider.GetProductsAsync();

            Assert.True(product.IsSuccess);
            Assert.True(product.Products.Any());
            Assert.Null(product.ErrorMessage);
        }


        [Fact]
        public async Task GetProductReturnsProductUsingValidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductReturnsProductUsingValidId))
                .Options;
            var dbContext = new ProductsDbContext(options);

            CreateProducts(dbContext);

            var productprofile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productprofile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var product = await productsProvider.GetProductAsync(1);

            Assert.True(product.IsSuccess);
            Assert.NotNull(product.Product);
            Assert.True(product.Product.Id == 1);
            Assert.Null(product.ErrorMessage);
        }

        [Fact]
        public async Task GetProductReturnsProductUsingInvalidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductReturnsProductUsingInvalidId))
                .Options;
            var dbContext = new ProductsDbContext(options);

            CreateProducts(dbContext);

            var productprofile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productprofile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var product = await productsProvider.GetProductAsync(-1);

            Assert.False(product.IsSuccess);
            Assert.Null(product.Product);
            Assert.NotNull(product.ErrorMessage);
        }

        private void CreateProducts(ProductsDbContext dbContext)
        {
            for (int i = 1; i <= 10; i++)
            {
                dbContext.Products.Add(new Product()
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString(),
                    Inventory = i + 10,
                    Price = (decimal)(i * 3.14)
                });
            }

            dbContext.SaveChanges();
        }
    }
}