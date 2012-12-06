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
    class App
    {
        public static String AdminEmail
        {
            get
            {

                return ConfigurationManager.AppSettings["AdminEmail"];
            }
        }

    }
}
