using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NeilGaiettoCom.Models
{
    public class StatsOverview
    {
        public int TotalView { get; set; }
        public int ViewsToday { get; set; }
        public DateTime LastView { get; set; }
    }
}