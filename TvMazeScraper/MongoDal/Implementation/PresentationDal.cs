using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDal.Configurations;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Interfaces;
using Shared.Models.Paging;
using Shared.Models.Presentation;

namespace MongoDal.Implementation
{
    public class PresentationDal : MongoContextBase, IPresentationDal
    {
        public IMongoCollection<TvShow> TvShowsCollection =>
            Database.GetCollection<TvShow>(MongoConfig.TvShowsCollectionName);

        public PresentationDal(MongoClient mongoClient,
            IOptions<MongoConfig> mongoConfig)
            : base(mongoClient, mongoConfig)
        {
        }

        public async Task InsertOrUpdateItemAsync(TvShow tvShow, CancellationToken cancellationToken)
        {
            if (tvShow == null)
            {
                throw new ArgumentException(nameof(tvShow));
            }
            
            await TvShowsCollection.ReplaceOneAsync(t => t.Id == tvShow.Id, tvShow, new UpdateOptions
            {
                IsUpsert = true
            }, cancellationToken);
        }

        public async Task<PaginatedResult<TvShow>> GetTvShowsWithActorsSortedDescendingAsync(TvShowRequest tvShowRequest, CancellationToken cancellationToken)
        {
            var filter = Builders<TvShow>.Filter.Empty;

            if (!string.IsNullOrEmpty(tvShowRequest.Name))
            {
                filter = Builders<TvShow>.Filter.Regex(nameof(tvShowRequest.Name), new BsonRegularExpression(tvShowRequest.Name));
            }

            var sort = Builders<TvShow>.Sort.Ascending(t => t.Id);

            var query = TvShowsCollection.Find(filter);

            var countTask = query.CountAsync(cancellationToken); // using deprecated method since looks like new 'CountDocumentAsync' is buggy
            
            var results = await query
                .Sort(sort)
                .Skip(tvShowRequest.Skip)
                .Limit(tvShowRequest.Take)
                .Project(x => new TvShow
                {
                    Id = x.Id,
                    Name = x.Name,
                    Cast = x.Cast.OrderByDescending(z => z.Birthday).ToArray()
                }).ToListAsync(cancellationToken);

             var count = await countTask;

            return new PaginatedResult<TvShow>((int)count, results);
        }
    }
}