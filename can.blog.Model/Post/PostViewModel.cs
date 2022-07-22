using System;
using System.Collections.Generic;
using System.Text;

namespace can.blog.Model.Post
{
    public class PostViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string OgImage { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategorySlug { get; set; }

        public string MetaDescription { get; set; }
        public string Author { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
