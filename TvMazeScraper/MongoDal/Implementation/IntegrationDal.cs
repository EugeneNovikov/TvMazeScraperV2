using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDal.Configurations;
using MongoDal.Models;
using MongoDB.Driver;
using Shared.Interfaces;
using Shared.Models.Integration;

namespace MongoDal.Implementation
{
    public class IntegrationDal : MongoContextBase,  IIntegrationDal
    {
        private readonly IMapper _mapper;

        public IntegrationDal(MongoClient mongoClient, 
            IOptions<MongoConfig> mongoConfig, IMapper mapper)
            : base (mongoClient, mongoConfig)
        {
            _mapper = mapper;
        }

        public IMongoCollection<HistoryRecordEntity> HistoryRecordsCollection =>
            Database.GetCollection<HistoryRecordEntity>(MongoConfig.HistoryRecordsCollectionName);
        
        public async Task<HistoryRecord> GetRecentHistoryRecordAsync(CancellationToken cancellationToken)
        {
            // mongo object Id sorted descending, it works since they have timestamp as part of it.
            var sortExpr = Builders<HistoryRecordEntity>.Sort.Descending(s => s.Id);

            var historyRecordEntity = await HistoryRecordsCollection.Find(Builders<HistoryRecordEntity>.Filter.Empty).Sort(sortExpr)
                .FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<HistoryRecord>(historyRecordEntity);
        }

        public async Task SaveHistoryRecordAsync(HistoryRecord historyRecord,
            CancellationToken cancellationToken)
        {
            var historyRecordEntity = _mapper.Map<HistoryRecordEntity>(historyRecord);
            
            await HistoryRecordsCollection.InsertOneAsync(historyRecordEntity, null, cancellationToken);
        }
    }
}
