using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace can.blog.Model.Tag
{
    public class TagCreateModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
