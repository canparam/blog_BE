using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace can.blog.Model.Post
{
    public class PostCreateModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string OgImage { get; set; }
        public string CategoryId { get; set; }
        public string MetaDescription { get; set; }
        public List<string> Tags { get; set; }
    }
}
