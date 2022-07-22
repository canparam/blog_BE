using System;
using System.IO;
using System.Threading.Tasks;
using can.blog.Infrastructure;
using can.blog.Model.Upload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace can.blog.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UploadController(IWebHostEnvironment hostEnvironment)
        {
            _webHostEnvironment = hostEnvironment;
        }

        [HttpPost]
        [Route("upload-image")]
        public IActionResult UploadImage(IFormFile file)
        {
            string uniqueFileName = null;
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;  
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);  
            using (var fileStream = new FileStream(filePath, FileMode.Create))  
            {  
                file.CopyTo(fileStream);  
            }
            string url = "/images/" + uniqueFileName;
            return StatusCode(200, new
            {
                status = StatusCodeRespon.SUCCESS,
                data = url
            });
        }
    }
}