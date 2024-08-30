using APIExplorer.ApiExplorer;
using APIExplorer.ApiExplorer.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

namespace APIExplorer
{
    public static class APIInitializer
    {
        public static IServiceCollection AddApiExplorer(this IServiceCollection services, Uri siteUrl)
        {
            var options = ApiExplorerOptions.CreateFromSiteURL(siteUrl);
            return services.AddApiExplorer(options);
        }
        public static IServiceCollection AddApiExplorer(this IServiceCollection services, string siteUrl)
        {
            var options = ApiExplorerOptions.CreateFromSiteURL(siteUrl);
            return services.AddApiExplorer(options);
        }
        public static IServiceCollection AddApiExplorer(this IServiceCollection services, Action<ApiExplorerOptions> configure)
        {
            var options = new ApiExplorerOptions();
            configure(options);
            return services.AddApiExplorer(options);
        }
        public static IServiceCollection AddApiExplorer(this IServiceCollection services, ApiExplorerOptions options)
        {
            string AuthName = "OptiContentCloud";
            var wellKnown = options.MasterUrl is not null ? new Uri(options.MasterUrl, ".well-known/openid-configuration") : new Uri("/.well-known/openid-configuration", UriKind.Relative);

            services.AddSingleton(options);

            //Add Swagger
            services.AddSwaggerGen(swaggerOptions => {
                swaggerOptions.SwaggerDoc(options.DocumentName, options.CreateApiInfo());
                swaggerOptions.ResolveConflictingActions(ContentTypeApiDescriptionConflictResolver.Resolve);
            });
            services.AddSwaggerGenNewtonsoftSupport();

            return services;
        }
        public static IApplicationBuilder UseApiExplorer(this IApplicationBuilder app, bool mapEndpoints = true)
        {
          var options = app.ApplicationServices.GetService<ApiExplorerOptions>();
            
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                var endpoint = $"/swagger/{options.DocumentName}/swagger.json";
                c.SwaggerEndpoint(endpoint, options.ApplicationName);
                c.HeadContent = "<style>.swagger-ui .topbar { display: none; } .swagger-ui .information-container.wrapper .info { margin: 20px 0px; }</style>";
                c.EnablePersistAuthorization();
            });

            if (mapEndpoints)
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapApiExplorer();
                });
            }

            return app;
        }
        public static IEndpointRouteBuilder MapApiExplorer(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapSwagger();
            return endpoints;
        }
    }
}
