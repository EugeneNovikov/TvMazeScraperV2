using System;
using ComponentRegistrar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NJsonSchema;
using NSwag.AspNetCore;
using PresentationApi.Configurations;
using Serilog;
using Shared.Middleware;

namespace PresentationApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var defaultDateTimeFormat = Configuration.GetSection("DefaultDateTimeFormat")?.Value ??
                                        throw new InvalidOperationException("Default date time is not specified in the configuration.");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = defaultDateTimeFormat;
            });
            

            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));

            services.RegisterCommonServices();

            services.RegisterServicesWithDal(Configuration);

            services.AddSwagger();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddSerilog();

            app.UseMiddleware(typeof(ExceptionHandlingMiddleware));

            app.UseSwaggerUi3WithApiExplorer(settings =>
            {
                settings.GeneratorSettings.DefaultPropertyNameHandling =
                    PropertyNameHandling.CamelCase;
            });

            app.UseMvc();
        }
    }
}
