using Microsoft.Extensions.DependencyInjection;
using MongoDal.Implementation;
using MongoDB.Driver;
using System.Security.Authentication;
using AutoMapper;
using IntegrationBl.Configurations;
using IntegrationBl.Factories;
using IntegrationBl.Services;
using Microsoft.Extensions.Configuration;
using MongoDal.Configurations;
using PresentationBl.Services;
using Shared.Interfaces;
using Shared.Services;
using Shared.Wrappers;

namespace ComponentRegistrar
{
    public static class RegistrarExtensions
    {

        public static void RegisterCommonServices(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeService, DateTimeService>();
            services.AddSingleton<IJsonConverterWrapper, JsonConverterWrapper>();
            services.AddSingleton<IUrlFormatService, UrlFormatService>();
            services.AddSingleton<IPolicyFactory, PolicyFactory>();
        }

        public static void RegisterServicesWithDal(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IUpdateService, UpdateService>();
            services.AddSingleton<IIntegrationDal, IntegrationDal>();
            services.AddSingleton<IPresentationDal, PresentationDal>();
            services.AddSingleton<IPresentationService, PresentationService>();
            services.AddSingleton<ITvShowUpdateService, TvShowUpdateService>();
            
            services.Configure<MongoConfig>(configuration.GetSection("MongoConfiguration"));
            services.Configure<TvMazeConfig>(configuration.GetSection("TvMazeConfiguration"));

            var mapper = CreateMapper();

            services.AddSingleton<IMapper>(mapper);

            services.AddSingleton<MongoClient>(provider => 
            {
                var connectionString = configuration.GetSection("MongoConfiguration:ConnectionString").Value;
                
                var settings = MongoClientSettings.FromUrl(
                    new MongoUrl(connectionString)
                );

                settings.SslSettings = new SslSettings {EnabledSslProtocols = SslProtocols.Tls12};

                var mongoClient = new MongoClient(settings);
                
                return mongoClient;
            });
        }

        private static IMapper CreateMapper()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(typeof(DalMappingProfile));
                mc.AddProfile(typeof(IntegrationBlMappingProfile));
            });

            return mappingConfig.CreateMapper();
            
        }
    }
}
