using can.blog.Data.Repository;
using can.blog.Entity.Identity;
using can.blog.Services.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace can.blog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var sqlConnectString = Configuration.GetConnectionString("DBContext");
            services.AddDbContext<BlogDbContext>(options => options.UseNpgsql(sqlConnectString));

            
            //indentity
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<BlogDbContext>()
                .AddDefaultTokenProviders();
            // Default Policy
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            
            //add depency

            services.Build(Configuration);

            services.AddHttpContextAccessor();

            // Adding Authentication
            services.Configure<JWT>(Configuration.GetSection("JWT"));

            //Adding Athentication - JWT
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,

                        ValidIssuer = Configuration["JWT:Issuer"],
                        ValidAudience = Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
                    };
                    o.Events = new JwtBearerEvents
                    {
                        OnChallenge = async context =>
                        {
                            // Call this to skip the default logic and avoid using the default response
                            context.HandleResponse();
                            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                            context.Response.Headers.Add("Access-Control-Allow-Methods", "GET,PUT,POST,DELETE");
                            context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
                            context.Response.ContentType = "application/json";
                            // Write to the response in any way you wish
                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                            {
                                message = "Vui lòng đăng nhập"
                            }));
                        }
                    };
                });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Place Info Service API",
                    Version = "v1",
                    Description = "Sample service for Learner",
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "PlaceInfo Services"));
        }
    }
}