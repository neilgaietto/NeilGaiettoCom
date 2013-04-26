using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NeilGaiettoCom.Models;

namespace NeilGaiettoCom.Controllers
{
    public class StatsController : ApiController
    {
        // GET api/<controller>
        public StatsOverview Get()
        {
            StatsOverview overview = new StatsOverview();
            overview.TotalView = 9000;
            overview.ViewsToday = 55;
            overview.LastView = DateTime.Now.AddHours(-1);
            return overview;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}