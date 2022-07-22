using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace can.blog.Entity.Identity
{
    public class User : IdentityUser<string>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override string Id { get; set; }
        public string FullName { get; set; }
        public int Type { get; set; }
        [DefaultValue(0)]
        public int IsDel { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
