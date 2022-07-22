using can.blog.Entity;
using can.blog.Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace can.blog.Data.Repository
{
    public class BlogDbContext : IdentityDbContext<User, IdentityRole,string>
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options): base(options)
        {


        }
        public override DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<TagPost> TagPosts { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<FileManager> FileManagers { get; set; }

    }
}
