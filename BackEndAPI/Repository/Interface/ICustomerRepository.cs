using BackEndAPI.Model;

namespace BackEndAPI.Repository.Interface
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAll();

        Task<Customer> Get(int id);


    }
}
