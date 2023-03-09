using BackEndAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace BackEndAPI.Data
{
    public class CustomerDBContext:DbContext
    {
        public DbSet<Customer> Customers {get; set; }    
    }
}
