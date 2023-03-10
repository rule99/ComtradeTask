using FrontEndMVC.Models;
using FrontEndMVC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FrontEndMVC.Controllers
{
    public class RewardController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ICustomerService _customerService;

        public RewardController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7119/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _customerService = new CustomerService(_httpClient);
        }
        // GET: Reward
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Confirm() 
        {
            return View();
        }   
        
        //public JsonResult FindCustomer(int id) 
        //{
        //    var result=_customerService.GetCustomerByID(id);
        //    return Json(result);
           
        //}

        public async Task<ActionResult> FindCustomerButtonCLicked(int id)
        {
            var result = await _customerService.GetCustomerByID(id);
            CustomerBO customerBO = result;
            if(customerBO==null)
            {
                TempData["error"] = "Customer not found";
                return View("Index");
            }
            TempData["odgovor"]=result;
            return View("Index", customerBO);
        }

        public async Task<ActionResult> FindCustomerButtonNameClicked(string name)
        {

            
            var result = await _customerService.GetCustomerByName(name);
            CustomerBO customerBO = result;
            if (customerBO == null)
            {
                TempData["error"] = "Customer not found";
                return View("Index");
            }
            TempData["odgovor"] = result;
            return View("Index", customerBO);
        }

        public async Task<ActionResult> RewardCustomer(int id)
        {
            string agent= HttpContext.Session["username"].ToString();
            string response=await _customerService.RewardCustomer(id, agent);
            if (response== "Forbid")
            {
                TempData["error"] = "Agent reached max customer rewarded";
                return View("Index");
            }
            if (response == "BadRequest")
            {
                TempData["error"] = "Customer already rewarded";
                return View("Index");
            }
            TempData["Message"] = "Customer Rewarded";
            return RedirectToAction("MainMenu","Home");
        }

        public async Task<ActionResult> ReturnCustomer(int id)
        {
            var result = await _customerService.GetCustomerByID(id);
            CustomerBO customerBO = result;
            if (customerBO == null)
            {
                TempData["error"] = "Customer not found";
                return View("Confirm");
            }
            if(customerBO.Agent==null)
            {
                TempData["error"] = "Customer was not rewarded";
                return View("Confirm");
            }
            if (customerBO.ReturnCustomer == 1)
            {
                TempData["error"] = "Customer already listed as returned";
                return View("Confirm");
            }
            TempData["odgovor"] = result;
            return View("Confirm", customerBO);
        }

    }
}