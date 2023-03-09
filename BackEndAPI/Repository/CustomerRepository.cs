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
    }
}
