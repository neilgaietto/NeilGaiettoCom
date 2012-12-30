using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NeilGaiettoCom.Controllers
{
    public class BlogController : Controller
    {
        //
        // GET: /Blog/

        public ActionResult Index()
        {
            Models.Post post = new Models.Post();
            post.Get(1);

            return View(post);
        }

        public ActionResult PostDetails(string PostID)
        {

            return View();
        }
    }
}
