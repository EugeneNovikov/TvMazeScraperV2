using System;
using Microsoft.Extensions.Options;
using MongoDal.Configurations;
using MongoDB.Driver;

namespace MongoDal.Implementation
{
    public class MongoContextBase
    {
        protected readonly MongoClient MongoClient;
        protected readonly MongoConfig MongoConfig;

        protected IMongoDatabase Database => MongoClient.GetDatabase(MongoConfig.DbName);

        protected MongoContextBase(MongoClient mongoClient, IOptions<MongoConfig> mongoConfig)
        {
            MongoClient = mongoClient;
            MongoConfig = mongoConfig?.Value ?? throw new ArgumentNullException(nameof(MongoConfig));
        }
    }
}