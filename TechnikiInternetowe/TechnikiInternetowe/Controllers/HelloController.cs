using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace TechnikiInternetowe.Controllers
{
    [RoutePrefix("Hello")]
    public class HelloController : Controller
    {
        [HttpGet]
        //[Route("default")]
        public string Index()
        {
            return "Hello World!";
        }

        [HttpGet]
        [Route("{name}")]
        public static string HelloName(string name)
        {
            string asd =  "Hello " + name + ". Nice to see u!";
            return asd;
        }

        
    }
}
