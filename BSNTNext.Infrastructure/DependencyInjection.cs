using BSNTNext.Application.Interfaces.Services;
using BSNTNext.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BSNTNext.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IAuthServices, AuthServices>();
           

            return services;
        }
    }
}