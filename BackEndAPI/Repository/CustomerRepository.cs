using AutoMapper;
using BackEndAPI.Data;
using BackEndAPI.Model;
using BackEndAPI.Model.BO;
using BackEndAPI.Repository.Interface;
using CustomerSoapService;
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

        public async Task<Customer> Get(int id)
        {
            throw new NotImplementedException();
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
