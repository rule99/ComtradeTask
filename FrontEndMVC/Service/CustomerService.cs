using FrontEndMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace FrontEndMVC.Service
{
    public class CustomerService : ICustomerService
    {

        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            
        }

        public async Task<CustomerBO> GetCustomerByID(int id)
        {

            var response = await _httpClient.GetAsync($"api/Customer/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<CustomerBO>();
        }

      
        public async Task<CustomerBO> GetCustomerByName(string name)
        {
            var response = await _httpClient.GetAsync($"api/Customer/{name}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<CustomerBO>();
        }

        public async Task<IEnumerable<CustomerBO>> GetCustomers()
        {
            var response = await _httpClient.GetAsync("api/Customer");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<IEnumerable<CustomerBO>>();
        }

        public async Task<string> RewardCustomer(int id,string agent)
        {
           
            var response = await _httpClient.PostAsync($"api/Customer/{id}", agent, new JsonMediaTypeFormatter());
           
            return response.StatusCode.ToString();
        }
    }
}