using System;
using System.Collections.Generic;
using System.Text;

namespace can.blog.Model.Categories
{
    public class CategoryViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MetaDescription { get; set; }
        public string Slug { get; set; }
        public string OgImage { get; set; }
    }
}
