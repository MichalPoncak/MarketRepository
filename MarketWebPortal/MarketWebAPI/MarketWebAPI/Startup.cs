using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MarketWebAPI.Common;
using MarketWebAPI.Features.Price;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;

namespace MarketWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<IDataAccessHelper, DataAccessHelper>();
            services.AddTransient<IPriceService, PriceService>();
            services.AddTransient<IPriceDataAccess, PriceDataAccess>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                //c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Market Web API",
                    Description = "Market Web API using .NET Core 3.1. It can read and bulk insert market prices with dates.",
                    Contact = new OpenApiContact
                    {
                        Name = "Michal Poncak",
                        Email = "Michal.Poncak@live.com",
                        Url = new Uri("https://www.linkedin.com/in/michal-poncak-33028454/?originalSubdomain=ie&challengeId=AQFmqYTL5pjHoAAAAXN2f4zsYboIEf5ydtt1cmFX-5rBa3WnVXdFMwpBJTY1zh8uGvT6uQ_JtDPRP909frCCevC_KWN4xfwVow&submissionId=2a9edead-e312-2416-6108-c20846d6802e"),
                    }
                });

                // this generates an XML documentation file
                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "MarketWebAPI.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
