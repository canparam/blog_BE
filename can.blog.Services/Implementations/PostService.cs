using can.blog.Data.Repository;
using can.blog.Entity;
using can.blog.Entity.Identity;
using can.blog.Infrastructure;
using can.blog.Model.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using can.blog.Model;

namespace can.blog.Services.Implementations
{
    public class PostService : BaseService<Posts>, IPostService
    {
        private readonly BlogDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly IPostTagService _postTagService;
        public PostService(BlogDbContext dbContext, IUnitOfWork unitOfWork, UserManager<User>  userManager, IPostTagService postTagService, IHttpContextAccessor httpContextAccessor) : base(dbContext)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _postTagService = postTagService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> CreatePost(PostCreateModel model)
        {
            var dbTras = _unitOfWork.BeginTransaction();

            try
            {
                
                var slug = FriendlyUrlHelper.GetFriendlyTitle(model.Title);
                if (!string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Slug.ConvertToUnSign();
                }
                var i = 0;
                // Prevent duplicate slugs
                while (GetAll().Any(cate => cate.Slug == slug))
                {
                    slug = $"{slug}-{++i}";
                }
                string userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = _dbContext.Users.FirstOrDefault(e => e.UserName == userName);
                var post = new Posts()
                {
                    Title = model.Title,
                    Slug = slug,
                    CategoryId = string.IsNullOrEmpty(model.CategoryId) == true ? null : model.CategoryId,
                    Content = model.Content,
                    OgImage = model.OgImage,
                    MetaDescription = model.MetaDescription,
                    UserId = user.Id,
                    CreatedDate = DateTime.Now
                };
                await InsertAsync(post);

                if (model.Tags.Count > 0)
                {
                    var tags = new List<TagPost>();
                    foreach (string idTag in model.Tags)
                    {
                        var tag = new TagPost()
                        {
                            PostId = post.Id,
                            TagId = idTag,
                        };
                        tags.Add(tag);
                    }
                   var t = _postTagService.InsertMulti(tags);
                }
                await _unitOfWork.CompleteAsync();

                dbTras.Commit();

                return post.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
                await dbTras.RollbackAsync();
               return null;
            }
        }

        public async Task<bool> DeletePost(Posts post)
        {
            Delete(post);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<PostViewModel> GetBySlug(string slug)
        {
            var query = await Query(p => p.Slug.Equals(slug))
                .Include(p => p.User)
                .Include(p => p.Categories)
                .FirstOrDefaultAsync();
            if (query == null)
            {
                return null;
            }

            var detail = MapperHelper.Map<Posts, PostViewModel>(query);
            return detail;
        }

        public async Task<PageOutput<PostViewModel>> GetList(string name, int limit, int page)
        {
            var query = GetAll().Where(x => string.IsNullOrWhiteSpace(name) ? true : x.Title.Contains(name)).OrderByDescending(e => e.CreatedDate)
                .Include(p => p.User)
                .Include(p => p.Categories);
            var totalPage = await query.CountAsync();
            var result = query.Paginate(page, limit);

            var output = MapperHelper.MapList<Posts, PostViewModel>(result.ToList());

            return new PageOutput<PostViewModel>(page, totalPage, output);
        }

        public Task<string> UpdatePost(PostUpdateModel model, Posts post)
        {
            throw new NotImplementedException();
        }
    }
}
