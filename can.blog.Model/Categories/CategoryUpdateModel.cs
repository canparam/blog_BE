using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace can.blog.Model.Categories
{
    public class CategoryUpdateModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Slug { get; set; }
        public string OgImage { get; set; }
    }
}
