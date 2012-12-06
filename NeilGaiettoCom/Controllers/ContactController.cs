using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NeilGaiettoCom.Controllers
{
    public class ContactController : Controller
    {
        //
        // GET: /Contact/


        public ActionResult Index()
        {
            return View(new Models.ContactForm());
        }

        [HttpPost]
        public ActionResult Index(Models.ContactForm contactForm)
        {
            return View();
        }
    }
}
