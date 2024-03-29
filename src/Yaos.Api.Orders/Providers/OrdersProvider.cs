﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Yaos.Api.Orders.Db;
using Yaos.Api.Orders.Interfaces;

namespace Yaos.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly ILogger<IOrdersProvider> logger;
        private readonly IMapper mapper;
        private readonly OrdersDbContext dbContext;

        public OrdersProvider(ILogger<IOrdersProvider> logger, IMapper mapper, OrdersDbContext dbContext)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.dbContext = dbContext;
            SeedData();
        }

        private void SeedData()
        {
            if (dbContext.Orders.Any())
                return;

            dbContext.Orders.Add(new Order()
            {
                Id = 1,
                CustomerId = 1,
                OrderDate = DateTime.Now,
                Items = new List<OrderItem>()
                {
                    new OrderItem() {OrderId = 1, ProductId = 1, Quantity = 10, UnitPrice = 10},
                    new OrderItem() {OrderId = 1, ProductId = 2, Quantity = 10, UnitPrice = 10},
                    new OrderItem() {OrderId = 1, ProductId = 3, Quantity = 10, UnitPrice = 10},
                    new OrderItem() {OrderId = 2, ProductId = 2, Quantity = 10, UnitPrice = 10},
                    new OrderItem() {OrderId = 3, ProductId = 3, Quantity = 1, UnitPrice = 100}
                },
                Total = 100
            });

            dbContext.Orders.Add(new Order()
            {
                Id = 2,
                CustomerId = 1,
                OrderDate = DateTime.Now.AddDays(-1),
                Items = new List<OrderItem>()
                {
                    new OrderItem() {OrderId = 1, ProductId = 1, Quantity = 10, UnitPrice = 10},
                    new OrderItem() {OrderId = 1, ProductId = 2, Quantity = 10, UnitPrice = 10},
                    new OrderItem() {OrderId = 1, ProductId = 3, Quantity = 10, UnitPrice = 10},
                    new OrderItem() {OrderId = 2, ProductId = 2, Quantity = 10, UnitPrice = 10},
                    new OrderItem() {OrderId = 3, ProductId = 3, Quantity = 1, UnitPrice = 100}
                },
                Total = 100
            });

            dbContext.Orders.Add(new Order()
            {
                Id = 3,
                CustomerId = 2,
                OrderDate = DateTime.Now,
                Items = new List<OrderItem>()
                {
                    new OrderItem() {OrderId = 1, ProductId = 1, Quantity = 10, UnitPrice = 10},
                    new OrderItem() {OrderId = 1, ProductId = 2, Quantity = 10, UnitPrice = 10},
                    new OrderItem() {OrderId = 1, ProductId = 3, Quantity = 1, UnitPrice = 100}
                },
                Total = 100
            });

            dbContext.SaveChanges();
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                var orders = await dbContext.Orders
                    .Where(o => o.CustomerId == customerId)
                    .Include(o => o.Items)
                    .ToListAsync();

                if(orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }

                return (false, null, "not found");

            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
