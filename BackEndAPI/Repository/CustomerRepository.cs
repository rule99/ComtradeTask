using AutoMapper;
using BackEndAPI.Data;
using BackEndAPI.Model;
using BackEndAPI.Model.BO;
using BackEndAPI.Repository.Interface;
using CustomerSoapService;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace BackEndAPI.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDBContext _db;
        private SOAPDemoSoapClient _SOAPDemoSoapClient;
        private readonly IMapper _mapper;
        public CustomerRepository(CustomerDBContext db, SOAPDemoSoapClient SOAPDemoSoapClient,IMapper mapper)
        {
            _SOAPDemoSoapClient = SOAPDemoSoapClient;
            _db = db;
           _mapper= mapper;
        }

        public async Task<CustomerBO> Get(int id)
        {
           CustomerBO customerBO = _mapper.Map<CustomerBO>(await _db.Customers.FirstOrDefaultAsync(c=>c.Id==id));
            if(customerBO == null) 
            {
                Person result = await _SOAPDemoSoapClient.FindPersonAsync(id.ToString());
                if(result == null) { return null; }
                customerBO =PersonToCustomerBOTransform(result,id);   
            }
            return customerBO;
        }

        private CustomerBO? PersonToCustomerBOTransform(Person result, int id)
        {

            CustomerBO cust = new CustomerBO()
            {
                Name = result.Name,
                Birth = result.DOB,
                SSN = result.SSN,
                Id = id,
                ReturnCustomer = 0,
                Home = new HomeBO()
                {
                    Adress = result.Home.Street,
                    City = result.Home.City,
                    Zip = result.Home.Zip
                }
            };
            return cust;
        }

        public async Task<List<Customer>> GetAll()
        {
          List<Customer> customersList = new List<Customer>();
            var result =await _SOAPDemoSoapClient.GetListByNameAsync("");
            foreach(PersonIdentification p in result) 
            {
                customersList.Add(_mapper.Map<Customer>(p));
            }
            return customersList;
        }

        public async Task<CustomerBO> GetByName(string name)
        {
            CustomerBO customerBO = _mapper.Map<CustomerBO>(await _db.Customers.FirstOrDefaultAsync(c => c.Name == name));
            if (customerBO == null)
            {
                PersonIdentification[] result = await _SOAPDemoSoapClient.GetListByNameAsync(name);
                if (result == null) { return null; }
                customerBO = _mapper.Map<CustomerBO>(result[0]);
            }
            return customerBO;
        }

        public async Task RewardCustomer(int id, string agent)
        {
            Person Pcustomer= await _SOAPDemoSoapClient.FindPersonAsync(id.ToString());
            if (Pcustomer == null) { return; }
            CustomerBO customerBO = PersonToCustomerBOTransform(Pcustomer, id);
            Home home = new Home()
            {
                Adress = customerBO.Home.Adress,
                City = customerBO.Home.City,
                Zip = customerBO.Home.Zip,

            };
            await _db.Homes.AddAsync(home);
            await _db.SaveChangesAsync();
            customerBO.Home.Id = home.Id;
            Customer customer = CustomerBoToCustomerTransformation(customerBO);
            customer.HomeID = home.Id;
            customer.DateRewarded = DateTime.Now;
            customer.AgentUserName = agent;
            await _db.Customers.AddAsync(customer);
            await _db.SaveChangesAsync();
            
        }

        private Customer CustomerBoToCustomerTransformation(CustomerBO customerBO)
        {
            Customer cu = new Customer()
            {
                Id = customerBO.Id,
                Birth = customerBO.Birth,
                SSN = customerBO.SSN,
                Name = customerBO.Name,

            };
             if (customerBO.Home.Id != 0)
            {
                cu.HomeID = customerBO.Home.Id;
            }
             return cu;
        }

        internal async Task<bool> AlreadyRewardCustomer(int id, string agent)
        {
            bool check = await _db.Customers.AnyAsync(c => c.Id == id);
            if (check)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal async Task<bool> AgentLimit(string agent)
        {
            int count=await _db.Customers.CountAsync(c=>c.AgentUserName==agent && c.DateRewarded.Date==DateTime.Now.Date);
            if(count>4) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal async Task<bool> AlreadyReturnedCustomer(int id)
        {
            Customer cus = await _db.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (cus.ReturnCustomer == 1)
            {
                return true;
            }
            else { return false; }
        }

        internal async Task<bool> CustomerNotRewarded(int id)
        {
            if(await _db.Customers.AnyAsync(c => c.Id == id))
            {
                return false;
            }
            else { return true; }

        }

        public async Task<CustomerBO> MarkAsReturned(int id)
        {
            Customer cus=await _db.Customers.FirstOrDefaultAsync(c=> c.Id == id);
            cus.ReturnCustomer = 1;
            await _db.SaveChangesAsync();
            CustomerBO cusBo=_mapper.Map<CustomerBO>(cus);
            return cusBo;
        }
    }
}
