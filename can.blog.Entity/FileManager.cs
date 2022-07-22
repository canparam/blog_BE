using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace can.blog.Entity
{
    public class FileManager : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        [Required]
        public string FileName { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Ext { get; set; }
    }
}