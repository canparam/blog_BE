using can.blog.Entity;
using can.blog.Model;
using can.blog.Model.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace can.blog.Services
{
    public interface ICategoriesService : IBaseService<Categories>
    {
        Task<PageOutput<CategoryViewModel>> GetList(string name, int limit, int page);

        Task<bool> DeleteCate(Categories cate);

        Task<Categories> UpdateCate(CategoryUpdateModel model, Categories cate);
        Task<Categories> CreateCate(CategoriesCreateModel model);
    }
}
