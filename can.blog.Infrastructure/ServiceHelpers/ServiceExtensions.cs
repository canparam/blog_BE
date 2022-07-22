using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace can.blog.Infrastructure.ServiceHelpers
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddScopeInterface(this IServiceCollection services, string interfaceSuffix, Assembly assembly)
        {
            var classTypes = assembly.GetTypes()
                 .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith(interfaceSuffix))
                 .ToArray();
            foreach (var classType in classTypes)
            {
                var interfaceType = classType.GetInterface("I" + classType.Name);
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, classType);
                }
            }
            return services;
        }
    }
}
