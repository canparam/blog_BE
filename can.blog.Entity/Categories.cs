using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace can.blog.Entity
{
    public class Categories : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string MetaDescription { get; set; }
        public string Slug { get; set; }
        public string OgImage { get; set; }

    }
}
