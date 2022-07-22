using can.blog.Infrastructure.ServiceHelpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace can.blog.Services
{
    public static class ServiceAssembly
    {
        public static Assembly Assembly => typeof(ServiceAssembly).Assembly;
        public static IServiceCollection ResolveServiceDependency(this IServiceCollection services)
        {
            return services.AddScopeInterface("Service", Assembly).AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
