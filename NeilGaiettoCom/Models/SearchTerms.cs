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
        //public static string CachePath { get { return HttpContext.Current.Server.MapPath("~\\cache"); } }
        public static string CachePath { get { return "E:\\Projects\\NeilGaiettoCom\\NeilGaiettoCom\\cache"; } }

        public SearchTerms()
        {

        }
        public IList<Term> GetNew()
        {
            List<Term> trends = new List<Term>();

            HtmlWeb source = new HtmlWeb();
            HtmlDocument doc = source.Load(trendsUrl);

            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a"))
            {
                trends.Add(new Term { Keyword = link.InnerText, Added = DateTime.Now });
            }

            return trends;
        }

        public IList<Term> GetExisting()
        {
            if (!BinaryRage.DB<List<Term>>.Exists("TermCache", CachePath))
            {
                return new List<Term>();
            }

            var listOfTerms = BinaryRage.DB<List<Term>>.Get("TermCache", CachePath, false);
            return listOfTerms;
        }

        public void UpdateTerms()
        {
            var existing = GetExisting();
            var newTerms = GetNew();
            //fix to remove dupes because of unique datetime
            var merged = existing.Union(newTerms.Where(x=>!existing.Contains(x))).Distinct().ToList();

            BinaryRage.DB<List<Term>>.Insert("TermCache", merged, CachePath, false);

        }

        public Term[] GetTerms(int take = 30)
        {
            UpdateTerms();
            var terms = GetExisting();
            return terms.OrderByDescending(x => x.Added).Take(take).ToArray();
        }
    }
}