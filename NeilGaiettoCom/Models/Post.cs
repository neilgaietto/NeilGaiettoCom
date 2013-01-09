using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NeilGaiettoCom.Models
{
    public class Post
    {
        public int PostID {get;set;}
        public string Title {get;set;}
        public string Slug {get;set;}
        public string ContentFile { get; set; }
        public DateTime PostDate { get; set; }
        public string[] Tags { get; set; }

        public void Get(string slug)
        {
            

            using (var db = new Data.NeilDBEntities())
            {
                var post = (from p in db.Posts
                            where p.Slug == slug
                            select p).FirstOrDefault();
                PostID = post.PostID;
                Title = post.Title;
                Slug = post.Slug;
                PostDate = post.PostDate.Value;
                ContentFile = post.ContentFile;


            }
        }
    }
}