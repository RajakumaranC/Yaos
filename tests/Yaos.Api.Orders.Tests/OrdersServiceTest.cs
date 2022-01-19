using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Yaos.Api.Orders.Db;
using Yaos.Api.Orders.Profiles;
using Yaos.Api.Orders.Providers;

namespace Yaos.Api.Orders.Tests
{
    public class OrdersServiceTest
    {
        private OrdersDbContext dbContext;
        private Mapper mapper;
        private OrdersProvider ordersProvider;
        public OrdersServiceTest()
        {
            var options = new DbContextOptionsBuilder<OrdersDbContext>().UseInMemoryDatabase(nameof(OrdersServiceTest))
                .Options;
            dbContext = new OrdersDbContext(options);

            var ordersprofile = new OrderProfile();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(ordersprofile));
            mapper = new Mapper(configuration);


            ordersProvider = new OrdersProvider(null, mapper, dbContext);
        }
        [Fact]
        public async Task GetOrdersReturnsAllOrdersAsync()
        {
            var order = await ordersProvider.GetOrdersAsync(1);

            Assert.True(order.IsSuccess);
            Assert.True(order.Orders.Any());
            Assert.True(order.Orders.First().CustomerId == 1);
            Assert.True(order.Orders.First().Items.Any());  
            Assert.Null(order.ErrorMessage);
        }

    }
}