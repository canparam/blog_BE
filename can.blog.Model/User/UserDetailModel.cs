using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace can.blog.Model.User
{
    public class UserDetailModel
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Token { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
