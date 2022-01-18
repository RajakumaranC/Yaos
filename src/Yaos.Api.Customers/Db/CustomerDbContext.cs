using Microsoft.EntityFrameworkCore;

namespace Yaos.Api.Customers.Db
{
    public class CustomerDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public CustomerDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
