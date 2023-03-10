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
            _customerRepository = customerRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<Customer>>> GetAll()
        {
            try
            {
                var result = await _customerRepository.GetAll();

                return Ok(result);
            }
            catch (Exception ex)
            {


                return BadRequest(ex);
            }

        }

        [HttpGet("{id:int}", Name = "GetByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBO>> GetByID(int id)
        {
            try
            {
                CustomerBO result = await _customerRepository.Get(id);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {

                    return NotFound(result);
                }

            }
            catch (Exception ex)
            {

                _response.Result = "Server Error" + ex;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                return BadRequest(ex);
            }


        }

        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBO>> GetByName(string name)
        {

            try
            {
                CustomerBO result = await _customerRepository.GetByName(name);
                if (result != null)
                {

                    return Ok(result);
                }
                else
                {

                    return NotFound(result);
                }

            }
            catch (Exception ex)
            {

                _response.Result = "Server Error" + ex;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                return BadRequest(ex);
            }


        }

        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> RewardCustomer(int id, [FromBody] string agent)
        {
            if (await _customerRepository.AlreadyRewardCustomer(id, agent))
            {
                return BadRequest("Already rewarded");
            }
            else if (await _customerRepository.AgentLimit(agent))
            {
                return Forbid("Agent reached max customer rewarded");
            }
            else
            {
                await _customerRepository.RewardCustomer(id, agent);
                return CreatedAtRoute(GetByID, new { id = id });

            }




        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> ConfirmReturnCustomer(int id)
        {
            try
            {

                if (await _customerRepository.CustomerNotRewarded(id))
                {
                    return NotFound("Customer was not rewarded");
                }
                else if (await _customerRepository.AlreadyReturnedCustomer(id))
                {
                    return BadRequest("Already returend customer");
                }
                else
                {
                    await _customerRepository.MarkAsReturned(id);
                    return NoContent();
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

    }
}
