using System;
using System.IO;
using System.Reflection;
using Geodesics.Api.Business;
using Geodesics.Api.Infrastructure.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Geodesics.Api
{
    public class Startup
    {
        private const string AppName = "Geodesics API";
        private const string AppVersion = "v1";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(options =>
            {
                var documentation = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var filePath = Path.Combine(AppContext.BaseDirectory, documentation);
                options.IncludeXmlComments(filePath);
                options.DocumentFilter<LowercaseDocumentFilter>();
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc(AppVersion,
                    new Info
                    {
                        Title = AppName,
                        Version = AppVersion
                    });
            });

            // Register your services here
            services.AddTransient<IDistanceService, DistanceService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            var rewriteOptions = new RewriteOptions();
            rewriteOptions.AddRedirect("^$", "swagger");
            app.UseSwagger(options => options.RouteTemplate = "swagger/{documentName}/swagger.json");
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"{AppVersion}/swagger.json", $"{AppName} {AppVersion}");
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRewriter(rewriteOptions);
        }
    }
}
