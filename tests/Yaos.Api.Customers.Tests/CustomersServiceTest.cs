using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Yaos.Api.Customers.Db;
using Yaos.Api.Customers.Profiles;
using Yaos.Api.Customers.Providers;

namespace Yaos.Api.Customers.Tests
{
    public class CustomersServiceTest
    {
        private CustomerDbContext dbContext;
        private Mapper mapper;
        private CustomersProvider customersProvider;
        public CustomersServiceTest()
        {
            var options = new DbContextOptionsBuilder<CustomerDbContext>().UseInMemoryDatabase(nameof(CustomersServiceTest))
                .Options;
            dbContext = new CustomerDbContext(options);

            var customerProfile = new CustomerProfile();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(customerProfile));
            mapper = new Mapper(configuration);

            //CreateCustomers(dbContext);

            customersProvider = new CustomersProvider(dbContext, null, mapper);
        }

        [Fact]
        public async Task GetCustomersReturnsAllCustomersAsync()
        {
            var customer = await customersProvider.GetCustomersAsync();

            Assert.True(customer.IsSuccess);
            Assert.True(customer.Customers.Any());
            Assert.Null(customer.ErrorMessage);
        }


        [Fact]
        public async Task GetCustomerReturnsCustomerUsingValidId()
        {
            var customer = await customersProvider.GetCustomerAsync(1);

            Assert.True(customer.IsSuccess);
            Assert.NotNull(customer.Customer);
            Assert.True(customer.Customer.Id == 1);
            Assert.Null(customer.ErrorMessage);
        }

        [Fact]
        public async Task GetCustomerReturnsCusotmerUsingInvalidId()
        {
            var customer = await customersProvider.GetCustomerAsync(-1);

            Assert.False(customer.IsSuccess);
            Assert.Null(customer.Customer);
            Assert.NotNull(customer.ErrorMessage);
        }

        private void CreateCustomers(CustomerDbContext dbContext)
        {
            for (int i = 1; i <= 10; i++)
            {
                dbContext.Customers.Add(new Customer()
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString(),
                    Address = Guid.NewGuid().ToString()
                });
            }

            dbContext.SaveChanges();
        }
    }
}

