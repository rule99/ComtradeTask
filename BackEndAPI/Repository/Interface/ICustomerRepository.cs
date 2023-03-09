using BackEndAPI.Model;
using BackEndAPI.Model.BO;

namespace BackEndAPI.Repository.Interface
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAll();

        Task<CustomerBO> Get(int id);

        Task<CustomerBO> GetByName(string name);

        Task RewardCustomer(int id, string agent);
    }
}
