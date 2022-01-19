using MasstransitConfig.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Saunter;
using WebApplicationSample.Config;

namespace WebApplicationSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services
                .AddSwaggerConfig()
                .AddAsyncApiConfig()
                .ConfigureRabbitmq(Configuration)
                .AddLogging()
                .AddRouting(option => option.LowercaseUrls = true);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            ConfigureSwagger(app);

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                ConfigureAsyncApi(endpoints);
            });
        }

        private static void ConfigureAsyncApi(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapAsyncApiDocuments();
            endpoints.MapAsyncApiUi();
        }

        private static void ConfigureSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
