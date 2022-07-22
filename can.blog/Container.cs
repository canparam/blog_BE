using can.blog.Infrastructure;
using can.blog.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace can.blog
{
    public static class Container
    {
        public static IServiceCollection Build(this IServiceCollection services, IConfiguration Configuration)
        {
            return services
             .InfrastructureDependency()
             .ResolveServiceDependency();
        }
        
    }
}
