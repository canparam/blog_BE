using can.blog.Infrastructure;
using can.blog.Model.Tag;
using can.blog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace can.blog.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }
        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> GetAll(string name, int limit = 20, int page = 1)
        {
            var result = await _tagService.GetList(name, limit, page);
            return StatusCode(200, result);
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(TagCreateModel model)
        {

            var res = await _tagService.CreateTag(model);
            return StatusCode(200, new
            {
                status = StatusCodeRespon.SUCCESS,
                data = res
            });
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var cate = await _tagService.GetAsyncById(id);
            if (cate == null)
            {
                return StatusCode(200, new
                {
                    message = "Không tìm thấy Danh mục",
                    status = StatusCodeRespon.FAIL
                });
            }
            var del = await _tagService.DeleteTag(cate);

            return StatusCode(200, new
            {
                message = "Xóa thành công!",
                status = StatusCodeRespon.SUCCESS
            });
        }

    }
}
