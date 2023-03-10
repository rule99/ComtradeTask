using FrontEndMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FrontEndMVC.Service
{
    internal interface ICustomerService
    {

        Task<IEnumerable<CustomerBO>> GetCustomers();
        Task<CustomerBO> GetCustomerByID(int id);
        Task<CustomerBO> GetCustomerByName(string name);
        Task<string> RewardCustomer(int id,string agent); 
        Task<string> MarkAsReturned(int id);
        


    }
}
