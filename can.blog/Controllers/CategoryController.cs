using can.blog.Entity;
using can.blog.Infrastructure;
using can.blog.Model.Categories;
using can.blog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace can.blog.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;
        public CategoryController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }
        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> GetAll(string name,int limit = 20, int page = 1)
        {
            var result = await _categoriesService.GetList(name, limit, page);
            return StatusCode(200,result);
        }
        [HttpPost]
        [Route("create")]

        public async Task<IActionResult> Create(CategoriesCreateModel model)
        {
            var cate = await _categoriesService.FindAsync(e => e.Slug == model.Slug);
            if (cate != null)
            {
                return StatusCode(200, new
                {
                    message = "Slug đã tồn tại",
                    status = StatusCodeRespon.FAIL
                });
            }
            var res = _categoriesService.CreateCate(model);
            return StatusCode(200, new
            {
               status = StatusCodeRespon.SUCCESS,
               data = res.Result
            });
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var cate = await _categoriesService.GetAsyncById(id);
            if (cate == null)
            {
                return StatusCode(200, new
                {
                    message = "Không tìm thấy Danh mục",
                    status = StatusCodeRespon.FAIL
                });
            }
            var del =  await _categoriesService.DeleteCate(cate);

            return StatusCode(200, new
            {
                message = "Xóa thành công!",
                status = StatusCodeRespon.SUCCESS
            });
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update(CategoryUpdateModel model)
        {
            var cate = await _categoriesService.GetAsyncById(model.Id);
            if (cate == null)
            {
                return StatusCode(200, new
                {
                    message = "Không tìm thấy Danh mục",
                    status = StatusCodeRespon.FAIL
                });
            }
           
            var update = _categoriesService.UpdateCate(model,cate);
            return StatusCode(200, new
            {
                message = "Cập nhật thành công!",
                status = StatusCodeRespon.SUCCESS,
                data = update.Result
            });
        }
    }
}
