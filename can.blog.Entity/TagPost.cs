using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace can.blog.Entity
{
    public class TagPost : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public string TagId { get; set; }
        public string PostId { get; set; }

        [ForeignKey(nameof(TagId))]
        public Tags Tag { get; set; }
    }
}
