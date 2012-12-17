using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NeilGaiettoCom.Models
{
    public class Post
    {
        public int PostID;
        public string Title;
        public string Slug;
        public DateTime? PostDate;
        public string[] Tags;

        public void Get(int postID)
        {
            PostID = postID;

            using (var db = new Data.NeilDBEntities())
            {
                var post = (from p in db.Posts
                            where p.PostID == PostID
                            select p).FirstOrDefault();
                Title = post.Title;
                Slug = post.URLPath;
                PostDate = post.PostDate;


            }
        }
    }
}