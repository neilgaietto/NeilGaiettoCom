//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NeilGaiettoCom.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Post
    {
        public Post()
        {
            this.PostTags = new HashSet<PostTag>();
        }
    
        public int PostID { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public Nullable<int> Views { get; set; }
        public string ContentFile { get; set; }
        public string Slug { get; set; }
    
        public virtual ICollection<PostTag> PostTags { get; set; }
    }
}
