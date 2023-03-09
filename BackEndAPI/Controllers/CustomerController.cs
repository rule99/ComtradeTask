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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

                _response.Result = "Server Error"+ ex;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                return BadRequest( _response);
            }
            return _response;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetByID(int id)
        {
            try
            {
                CustomerBO result = await _customerRepository.Get(id);
                if (result != null)
                {
                    _response.Result = result;
                    _response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    _response.Result = "Customer not FOund";
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
               
            }
            catch (Exception ex)
            {

                _response.Result = "Server Error" + ex;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
           
            return _response;
        }

    }
}
