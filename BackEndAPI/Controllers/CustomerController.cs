using AutoMapper.Configuration.Conventions;
using BackEndAPI.Model.BO;
using CustomerSoapService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace BackEndAPI.Controllers
{
    [Route("api/Customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private SOAPDemoSoapClient _SOAPDemoSoapClient;
        public CustomerController(SOAPDemoSoapClient SOAPDemoSoapClient)
        {
            _SOAPDemoSoapClient=SOAPDemoSoapClient;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var results=await _SOAPDemoSoapClient.GetListByNameAsync("");
            return Ok(results);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetAll(int id)
        {
            Person result = await _SOAPDemoSoapClient.FindPersonAsync(id.ToString());
            
            return Ok(result);
        }

    }
}
