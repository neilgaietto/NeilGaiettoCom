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
        public JsonResult Index(Models.ContactForm contactForm)
        {

            Models.ResponseMessage respMsg = new Models.ResponseMessage();
            //if (!ModelState.IsValid)//TODO: fix validation
            //{
            //    respMsg.Status = 0;
            //    respMsg.Message = "";
            //}

            try
            {
                Data.App.SendContactForm(contactForm);
                respMsg.Status = 1;
                respMsg.Message = "";
            }
            catch (Exception ex)//handle error message based on error
            {
                respMsg.Status = 0;
                respMsg.Message = "Error submitting form. Please try again later or email me at <a href=\"mailto:neil.gaietto@gmail.com\">neil.gaietto@gmail.com</a>";
            }


            return Json(respMsg);
        }
    }
}
