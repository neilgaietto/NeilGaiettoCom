using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace NeilGaiettoCom.Data
{
    /// <summary>
    /// A place to keep all the cache objects.
    /// </summary>
    public static class CacheBucket
    {
        public static Cache Current
        {
            get
            {
                return HttpRuntime.Cache;
            }
        }


		/*
        public static Dictionary<int, Models.Product> CachedProducts
        {
            get
            {
                if (!(Current["CachedProducts"] is Dictionary<int, Models.Product>))
                {
                    Current["CachedProducts"] = new Dictionary<int, Models.Product>();
                }
                return Current["CachedProducts"] as Dictionary<int, Models.Product>;
            }
            set
            {
                Current["CachedProducts"] = value;
            }
        }
		 */
    }


}
