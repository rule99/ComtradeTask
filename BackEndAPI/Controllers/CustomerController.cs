using AutoMapper.Configuration.Conventions;
using BackEndAPI.Model;
using BackEndAPI.Model.BO;
using BackEndAPI.Repository;
using CustomerSoapService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Customer>>> GetAll()
        {
            try
            {
                var result = await _customerRepository.GetAll();

                //Ovaj deo sam morao da dodam posto sam rzlicito nazvao kolone i kako ne bih trazio svuda u kodu
                List<CustomerBO> list = new List<CustomerBO>();
                foreach (var item in result)
                {
                    CustomerBO cus = new CustomerBO()
                    {
                        Id = item.Id,
                        Birth = item.Birth,
                        SSN = item.SSN,
                        Name = item.Name,
                        Agent = item.AgentUserName,
                        Home = new HomeBO()
                        {
                            Id = item.HomeID,
                            Adress = item.Home.Adress,
                            City = item.Home.City,
                            Zip = item.Home.Zip
                        }
                    };
                    list.Add(cus);
                }

                return Ok(list);
            }
            catch (Exception ex)
            {


                return StatusCode(500,ex);
            }

        }

        [HttpGet("{id:int}", Name = "GetByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

             
                return StatusCode(500, ex); 
            }


        }

        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

              
                return  StatusCode(500, ex);
            }


        }

        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RewardCustomer(int id, [FromBody] string agent)
        {
            try
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
            catch (Exception ex)
            {
                
                return StatusCode(500,ex);
            }




        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

                return StatusCode(500, ex);
            }
        }

    }
}
