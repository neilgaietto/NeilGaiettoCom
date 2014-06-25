using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NeilGaiettoCom.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TestCSV()
        {
            Response.AddHeader("content-disposition", "attachment;filename=test.csv");
            Response.ContentType = "text/csv";
            Response.TransmitFile("~/content/files/test.csv");
            return null;
        }

        
        public JsonResult IpCompute(string ip)
        {
            Int64 retIP = -1;

            string[] seg = ip.Split('.');
            Int64 A, B, C, D;
            if (Int64.TryParse(seg[0], out A) && Int64.TryParse(seg[1], out B) && Int64.TryParse(seg[2], out C) && Int64.TryParse(seg[3], out D))
                retIP = (((A * 256 + B) * 256 + C) * 256 + D);

            return Json(retIP.ToString(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Andrea()
        {
            return View();
        }


    }
}
