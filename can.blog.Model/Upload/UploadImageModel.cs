using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace can.blog.Model.Upload
{
    public class UploadImageModel
    {
        [Required(ErrorMessage = "Please choose image")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}