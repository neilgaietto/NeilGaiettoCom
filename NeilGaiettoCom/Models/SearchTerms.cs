using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace NeilGaiettoCom.Models
{
    public class SearchTerms
    {
        string trendsUrl = "http://www.google.com/trends/hottrends/atom/hourly";
        public SearchTerms()
        {

        }
        public Term[] GetNew()
        {
            List<Term> trends = new List<Term>();

            HtmlWeb source = new HtmlWeb();
            HtmlDocument doc = source.Load(trendsUrl);

            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a"))
            {
                trends.Add(new Term { Keyword = link.InnerText, Added = DateTime.Now });
            }

            return trends.ToArray();
        }

        public Term[] GetExisting()
        {
            return new Term[0];
        }

        public Term[] GetTerms(int take = 20)
        {
            var terms = GetNew().Union(GetExisting());
            return terms.OrderByDescending(x => x.Added).Take(take).ToArray();
        }
    }
}