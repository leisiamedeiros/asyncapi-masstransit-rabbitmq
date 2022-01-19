using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace WebApplicationSample.Config
{
    public static class Swagger
    {
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Web API Sample",
                    Description = "An ASP.NET Core Web API sample"
                });
            });

            return services;
        }
    }
}
