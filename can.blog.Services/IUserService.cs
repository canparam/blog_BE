using can.blog.Entity;
using can.blog.Entity.Identity;
using can.blog.Model.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace can.blog.Services
{
    public interface IUserService : IBaseService<User>
    {
        RefreshToken CreateRefreshToken();
        Task<UserDetailModel> GetTokenAsync(LoginModel model);
        Task<UserDetailModel> RefreshTokenAsync(string token);

    }
}
