using BackEndAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace BackEndAPI.Data
{
    public class CustomerDBContext:DbContext
    {
        public CustomerDBContext(DbContextOptions<CustomerDBContext> options):base(options)
        {
            
        }

        public DbSet<Customer> Customers {get; set; }    
    }
}
