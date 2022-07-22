using can.blog.Entity;
using can.blog.Model;
using can.blog.Model.Tag;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace can.blog.Services
{
    public interface ITagService : IBaseService<Tags>
    {
        Task<PageOutput<TagViewModel>> GetList(string name, int limit, int page);


        Task<Tags> CreateTag(TagCreateModel model);
        Task<bool> DeleteTag(Tags tag);
    }
}
