using can.blog.Entity;
using can.blog.Model;
using can.blog.Model.Post;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace can.blog.Services
{
    public interface IPostService : IBaseService<Posts>
    {
        Task<string> CreatePost(PostCreateModel model);

        Task<PostViewModel> GetBySlug(string slug);

        Task<PageOutput<PostViewModel>> GetList(string name, int limit, int page);

        Task<bool> DeletePost(Posts post);
        Task<string> UpdatePost(PostUpdateModel model, Posts post);
    }
}
