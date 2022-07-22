using can.blog.Infrastructure.ServiceHelpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace can.blog.Infrastructure
{
    public static class InfrastructureAssembly
    {
        public static Assembly Assembly => typeof(InfrastructureAssembly).Assembly;
        public static IServiceCollection InfrastructureDependency(this IServiceCollection services)
        {
            return services.AddScopeInterface("Service", Assembly);
        }
    }
}
