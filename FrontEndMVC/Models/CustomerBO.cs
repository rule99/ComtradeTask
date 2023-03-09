using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEndMVC.Models
{
    public class CustomerBO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Birth { get; set; }

        public string SSN { get; set; }

        public HomeBO Home { get; set; }

        public int ReturnCustomer { get; set; }
    }
}