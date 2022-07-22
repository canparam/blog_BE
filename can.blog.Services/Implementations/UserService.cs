using can.blog.Entity;
using can.blog.Model.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using can.blog.Entity.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using can.blog.Services.Settings;
using Microsoft.Extensions.Options;
using can.blog.Data.Repository;
using Microsoft.Extensions.Logging;

namespace can.blog.Services.Implementations
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly JWT _jwt;
        private readonly BlogDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;
        public UserService(UserManager<User> userManager, IOptions<JWT> jwt, BlogDbContext dbContext, IUnitOfWork unitOfWork, ILogger<UserService> logger) : base(dbContext)
        { 
            _userManager = userManager;
            _jwt = jwt.Value;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public RefreshToken CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var generator = new RNGCryptoServiceProvider())
            {
                generator.GetBytes(randomNumber);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    Expires = DateTime.UtcNow.AddDays(10),
                    Created = DateTime.UtcNow
                };
            }
        }

        public async Task<UserDetailModel> GetTokenAsync(LoginModel model)
        {
            var userDetail = new UserDetailModel();
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                userDetail.IsAuthenticated = true;
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
                userDetail.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                if (user.RefreshTokens.Any(a => a.IsActive))
                {
                    var activeRefreshToken = user.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
                    userDetail.RefreshToken = activeRefreshToken.Token;
                    userDetail.RefreshTokenExpiration = activeRefreshToken.Expires;
                }
                else
                {
                    var refreshToken = CreateRefreshToken();
                    userDetail.RefreshToken = refreshToken.Token;
                    userDetail.RefreshTokenExpiration = refreshToken.Expires;
                    var dbTras = _unitOfWork.BeginTransaction();
                    try
                    {
                        user.RefreshTokens.Add(refreshToken);
                        Update(user);
                        _unitOfWork.Complete();
                        await dbTras.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                        await dbTras.RollbackAsync();
                        return null;
                    }
                }
                return userDetail;

            }
            else
            {
                userDetail.IsAuthenticated = false;
                userDetail.Message = "Sai tên đăng nhập hoặc mật khẩu!";
                return userDetail;
            }

        }

        public async Task<UserDetailModel> RefreshTokenAsync(string token)
        {
            var userDetail = new UserDetailModel();

            var user = GetAll().SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                userDetail.IsAuthenticated = false;
                userDetail.Message = "Token did not match any users.";
                return userDetail;
            }
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (refreshToken.IsActive)
            {
                userDetail.IsAuthenticated = false;
                userDetail.Message = "Token Not Active.";
                return userDetail;
            }
            //Revoke Current Refresh Token
            refreshToken.Revoked = DateTime.Now;
            //Generate new Refresh Token and save to Database
            var newRefreshToken = CreateRefreshToken();
            var dbTras = _unitOfWork.BeginTransaction();
            try
            {
                user.RefreshTokens.Add(newRefreshToken);
                Update(user);
                _unitOfWork.Complete();
                await dbTras.CommitAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                await dbTras.RollbackAsync();
                return null;
            }
            //Generates new jwt
            userDetail.IsAuthenticated = true;
            JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
            userDetail.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            userDetail.RefreshToken = newRefreshToken.Token;
            userDetail.RefreshTokenExpiration = newRefreshToken.Expires;
            return userDetail;


        }

        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

    }
}
