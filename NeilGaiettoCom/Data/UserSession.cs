using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace NeilGaiettoCom.Data
{
    /// <summary>
    /// Access to user's session properties.
    /// </summary>
    public static class UserSession
    {
        public static HttpSessionState Current
        {
            get
            {
                return HttpContext.Current.Session;
            }
        }

        public static object GeoLocation
        {
            get
            {
                return Current["GeoLocation"] as object;
            }
            set
            {
                Current["GeoLocation"] = value;
            }
        }

    }
}
