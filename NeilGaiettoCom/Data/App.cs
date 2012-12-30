using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Configuration;

namespace NeilGaiettoCom.Data
{
    /// <summary>
    /// The Current App's Properties.
    /// </summary>
    public class App
    {
        public static String AdminEmail
        {
            get
            {

                return ConfigurationManager.AppSettings["AdminEmail"];
            }
        }

        public static void PageVisited(string page, string visitor)
        {
            try
            {
                using (var db = new Data.NeilDBEntities())
                {
                    PageVisit pv = new PageVisit()
                    {
                        Page = page,
                        Visitor = visitor,
                        When = DateTime.Now
                    };
                    db.PageVisits.Add(pv);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
