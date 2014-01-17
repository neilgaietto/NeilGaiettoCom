using NeilGaiettoCom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NeilGaiettoCom.Controllers
{
    public class BadaController : Controller
    {
        //
        // GET: /Bada/

        public ActionResult Index()
        {
            var model = new BadaView();
            var trends = new SearchTerms().GetTerms();
            model.Trends = trends;
            return View(model);
        }

        public ActionResult Bing()
        {
            return View();
        }

        public ActionResult Boom()
        {
            return View();
        }

        
        public JsonResult Terms()
        {
            var trends = new SearchTerms().GetTerms();
            return Json(trends);
        }
    }
}
