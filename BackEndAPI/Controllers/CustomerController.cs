using AutoMapper.Configuration.Conventions;
using BackEndAPI.Model;
using BackEndAPI.Model.BO;
using BackEndAPI.Repository;
using CustomerSoapService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.WebSockets;

namespace BackEndAPI.Controllers
{
    [Route("api/Customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        protected APIResponse _response;
        private CustomerRepository _customerRepository;
        public CustomerController(CustomerRepository customerRepository)
        {
            _response = new();
            _customerRepository=customerRepository;
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAll()
        {
            try
            {
                var result = await _customerRepository.GetAll();
                _response.Result = result;
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {

                _response.Result = "Server Error";
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetAll(int id)
        {
           
            
            return Ok();
        }

    }
}
