using AutoMapper.QueryableExtensions;
using can.blog.Data.Repository;
using can.blog.Entity;
using can.blog.Infrastructure;
using can.blog.Model;
using can.blog.Model.Categories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace can.blog.Services.Implementations
{
    public class CategoriesService : BaseService<Categories>, ICategoriesService
    {
        private readonly BlogDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        public CategoriesService(BlogDbContext dbContext, IUnitOfWork unitOfWork) :base(dbContext)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;

        }

        public async Task<Categories> CreateCate(CategoriesCreateModel model)
        {
            var slug = FriendlyUrlHelper.GetFriendlyTitle(model.Name);
            if (!string.IsNullOrWhiteSpace(model.Slug))
            {
                slug = model.Slug.ConvertToUnSign();;
            }
            var i = 0;
            // Prevent duplicate slugs
            while (GetAll().Any(cate => cate.Slug == slug))
            {
                slug = $"{slug}-{++i}";
            }

            var entity = new Categories()
            {
                Name = model.Name,
                Slug = slug,
                OgImage = model.OgImage,
                MetaDescription = model.MetaDescription,
                CreatedDate = DateTime.Now
            };
            var cate = await InsertAsync(entity);
            await _unitOfWork.CompleteAsync();
            return cate;
        }

        public async Task<bool> DeleteCate(Categories cate)
        {
            Delete(cate);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<PageOutput<CategoryViewModel>> GetList(string name, int limit, int page)
        {
            var query = GetAll().Where(x => string.IsNullOrWhiteSpace(name) ? true : x.Name.Equals(name)).OrderByDescending(e => e.CreatedDate);
            var totalPage = await query.CountAsync();
            var result = query.Paginate(page,limit);
            return new PageOutput<CategoryViewModel>(page,totalPage, result.ToList().Select(MappingEntity).ToList());
        }

        public async Task<Categories> UpdateCate(CategoryUpdateModel model, Categories cate)
        {
            cate.Slug = model.Slug;
            cate.ModifiedDate = DateTime.Now;
            cate.Name = model.Name;
            cate.OgImage = model.OgImage;
            await _unitOfWork.CompleteAsync();
            return cate;
        }

        private CategoryViewModel MappingEntity(Categories cate)
        {
            return new CategoryViewModel()
            {
                Id = cate.Id,
                Name = cate.Name,
                Slug= cate.Slug,
                MetaDescription  = cate.MetaDescription,
                OgImage =  cate.OgImage
            };
        }
     
    }
}
