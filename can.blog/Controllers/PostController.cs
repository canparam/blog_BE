using can.blog.Infrastructure;
using can.blog.Model.Post;
using can.blog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace can.blog.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> List(string name, int limit = 20, int page = 1)
        {
            var result = await _postService.GetList(name, limit, page);
            return StatusCode(200, result);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(PostCreateModel model)
        {
            var result = await _postService.CreatePost(model);
            if (result == null)
            {
                return StatusCode(200, new
                {
                    message = "Có lỗi xảy ra vui lòng thử lại",
                    status = StatusCodeRespon.ERROR
                });
            }
            return StatusCode(200, new
            {
                data = result,
                status = StatusCodeRespon.SUCCESS
            });
        }

        [HttpGet]
        [Route("detail")]
        public async Task<IActionResult> Detail(string slug)
        {
            var result =  await _postService.GetBySlug(slug);
            if (result == null)
            {
                return StatusCode(200, new
                {
                    message = "Không tìm thấy bài viết",
                    status = StatusCodeRespon.FAIL
                });
            }
            return StatusCode(200, new
            {
                data = result,
                status = StatusCodeRespon.SUCCESS
            });
        }

    }
}
