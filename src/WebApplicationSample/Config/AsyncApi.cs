using MasstransitConfig.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Saunter;
using Saunter.AsyncApiSchema.v2;
using System;

namespace WebApplicationSample.Config
{
    public static class AsyncApi
    {
        public static IServiceCollection AddAsyncApiConfig(this IServiceCollection services)
        {
            services.AddAsyncApiSchemaGeneration(options =>
            {
                options.AssemblyMarkerTypes = MarkerTypes();

                options.Middleware.Route = "/asyncapi/asyncapi.json";
                options.Middleware.UiBaseRoute = "/asyncapi/";
                options.Middleware.UiTitle = "My AsyncAPI Documentation";

                options.AsyncApi = new AsyncApiDocument
                {
                    Info = new Info("WebApplicationSample API", "1.0.0")
                    {
                        Description = "An ASP.NET Core Web API sample"
                    },
                    Servers =
                    {
                        { "rabbitmq", new Server("localhost", "amqp") }
                    }
                };
            });

            return services;
        }

        private static Type[] MarkerTypes()
        {
            return new[] {
                typeof(Startup),
                typeof(ContactCreatedEventConsumer),
            };
        }
    }
}
