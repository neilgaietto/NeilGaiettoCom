﻿using System;
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


            return View();
        }

        public ActionResult Post(string id)
        {
            Models.Post post = new Models.Post();
            post.Get(id);
            return View(post);
        }
    }
}
