using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Yaos.Api.Customers.Db;
using Yaos.Api.Customers.Interfaces;
using Yaos.Api.Customers.Models;

namespace Yaos.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomerDbContext dbContext;
        private readonly ILogger<CustomersProvider> logger;
        private readonly IMapper mapper;

        public CustomersProvider(CustomerDbContext dbContext, ILogger<CustomersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (dbContext.Customers.Any())
                return;

            dbContext.Customers.Add(new Db.Customer() { Id = 1, Name = "Raj", Address = "Vasantha Nagar, 3rd Street, Madurai" });
            dbContext.Customers.Add(new Db.Customer() { Id = 2, Name = "Kasi", Address = "Kanpalayam, 3rd Street, Madurai" });
            dbContext.Customers.Add(new Db.Customer() { Id = 3, Name = "Srinivasan", Address = "Kanpalayam, 3rd Street, Madurai" });
            dbContext.Customers.Add(new Db.Customer() { Id = 4, Name = "Ashay", Address = "Somewhere in Kanpur, UttarPradhesh" });
            dbContext.SaveChanges();
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                logger?.LogInformation("Querying Customers");
                var customers = await dbContext.Customers.ToListAsync();

                if(customers != null && customers.Any())
                {
                    logger?.LogInformation($"{customers.Count} customer(s) found");
                    var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(customers);
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

        public async Task<(bool IsSuccess, Models.Customer Customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                logger?.LogInformation($"Querying Customer with Id {id}");
                var customer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);

                if (customer != null)
                {
                    logger?.LogInformation($"{customer.Name} customer(s) found");
                    var result = mapper.Map<Db.Customer, Models.Customer>(customer);
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
