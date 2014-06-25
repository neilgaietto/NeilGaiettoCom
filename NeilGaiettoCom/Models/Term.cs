using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NeilGaiettoCom.Models
{
    [Serializable]
    public class Term
    {
        public string Keyword { get; set; }
        public DateTime Added { get; set; }
    }
}