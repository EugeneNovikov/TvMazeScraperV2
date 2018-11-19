using ComponentRegistrar;
using IntegrationApi.HostedServices;
using IntegrationBl.Clients;
using IntegrationBl.Configurations;
using IntegrationBl.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Shared.Middleware;

namespace IntegrationApi
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            services.AddHttpClient<ITvMazeClient, TvMazeClient>();

            services.Configure<IntegrationTasksConfig>(Configuration.GetSection("IntegrationTasksConfig"));

            services.Configure<PoliciesConfig>(Configuration.GetSection("PoliciesConfig"));

            services.AddHostedService<TasksExecutorHostedService>();

            services.AddSingleton<IWorkloadService, WorkloadService>();

            services.RegisterCommonServices();

            services.RegisterServicesWithDal(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddSerilog();

            app.UseMiddleware(typeof(ExceptionHandlingMiddleware));

            app.UseMvc();
        }
    }
}
