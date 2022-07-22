using can.blog.Entity;
using can.blog.Entity.Identity;
using can.blog.Helpers;
using can.blog.Model.User;
using can.blog.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace can.blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        public AuthenticateController(UserManager<User> userManager, IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            var result = await _userService.GetTokenAsync(model);
            if (result.IsAuthenticated)
            {
                return Ok(new
                {
                    Token = result.Token,
                    RefreshToken = result.RefreshToken,
                    RefreshTokenExpiration = result.RefreshTokenExpiration,
                });
            }
            return Unauthorized(new Response()
            {
                Status = 401,
                Message = "Sai tên đăng nhập hoặc mật khẩu"
            });
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            var userUsername = await _userManager.FindByNameAsync(model.Username);
            if (userUsername != null)
            {
                return Unauthorized(new Response()
                {
                    Status = 401,
                    Message = "Tên đăng nhập đã có người sử dụng"
                });
            }
            var userEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userEmail != null)
            {
                return Unauthorized(new Response()
                {
                    Status = 401,
                    Message = "Email đã có người sử dụng"
                });
            }
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok(new Response()
                {
                    Status = 200,
                    Message = "Đăng ký thành công!"
                });
            }
            return BadRequest();
            
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var response = await _userService.RefreshTokenAsync(refreshToken);
            if (response.IsAuthenticated)
            {
                return Ok(response);
            }
            return Unauthorized(new Response()
            {
                Status = 401,
                Message = response.Message
            });
        }
       

    }
}
