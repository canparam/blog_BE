using can.blog.Data.Repository;
using can.blog.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace can.blog.Services.Implementations
{
    public class PostTagService : BaseService<TagPost>, IPostTagService
    {
        private readonly BlogDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        public PostTagService(BlogDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;

        }
    }
}
