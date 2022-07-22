using can.blog.Entity.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace can.blog.Entity
{
    public class Posts : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Slug { get; set; }
        public string Content { get; set; }
        public string OgImage { get; set; }
        public string CategoryId { get; set; }
        public string MetaDescription { get; set; }
        public string UserId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Categories Categories { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public string GetAuthor => User?.FullName;
        public string GetCategoryName => Categories?.Name;
        public string GetCategorySlug => Categories?.Slug;

    }
}
