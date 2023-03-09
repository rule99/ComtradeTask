using BackEndAPI.Data;
using BackEndAPI.Model;
using BackEndAPI.Repository.Interface;

using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace BackEndAPI.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDBContext _db;
        

        public CustomerRepository(CustomerDBContext db)
        {
            _db = db;
           
        }

        public Task<Customer> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Customer>> GetAll()
        {
          
            throw new NotImplementedException();
        }
    }
}
