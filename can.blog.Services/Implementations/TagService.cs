using can.blog.Data.Repository;
using can.blog.Entity;
using can.blog.Infrastructure;
using can.blog.Model;
using can.blog.Model.Tag;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace can.blog.Services.Implementations
{
    public class TagService : BaseService<Tags>, ITagService
    {
        private readonly BlogDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        public TagService(BlogDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;

        }
        public async Task<Tags> CreateTag(TagCreateModel model)
        {
            var slug = FriendlyUrlHelper.GetFriendlyTitle(model.Name);

            var i = 0;
            // Prevent duplicate slugs
            while (GetAll().Any(tag => tag.Slug == slug))
            {
                slug = $"{slug}-{++i}";
            }

            var tag = new Tags()
            {
                Name = model.Name,
                CreatedDate = DateTime.Now,
                Slug = slug
            };
            var res = await InsertAsync(tag);
            await _unitOfWork.CompleteAsync();
            return res;
        }

        public async Task<bool> DeleteTag(Tags tag)
        {
            Delete(tag);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<PageOutput<TagViewModel>> GetList(string name, int limit, int page)
        {
            var query = GetAll().Where(x => string.IsNullOrWhiteSpace(name) ? true : x.Name.Equals(name)).OrderByDescending(e => e.CreatedDate)
                .Include(e => e.TagPosts);
            var totalPage = await query.CountAsync();
            var result = query.Paginate(page, limit);
            return new PageOutput<TagViewModel>(page, totalPage, result.ToList().Select(MappingEntity).ToList());
        }


        private TagViewModel MappingEntity(Tags tag)
        {
            return new TagViewModel()
            {
                Id = tag.Id,
                Name = tag.Name,
                Slug = tag.Slug,
                CountTotalPost = tag.TagPosts.Count,
            };
        }
    }
}
