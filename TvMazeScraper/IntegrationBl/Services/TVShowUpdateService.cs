using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IntegrationBl.Clients;
using Shared.Exceptions;
using Shared.Interfaces;
using Shared.Models.Presentation;
using Shared.Services;

namespace IntegrationBl.Services
{
    public class TvShowUpdateService : ITvShowUpdateService
    {
        private readonly IMapper _mapper;
        private readonly ITvMazeClient _tvMazeClient;
        private readonly IPresentationDal _presentationDal;
        private readonly IIntegrationDal _integrationDal;
        private readonly IDateTimeService _dateTimeService;

        public TvShowUpdateService(IMapper mapper, 
            ITvMazeClient tvMazeClient,
            IPresentationDal presentationDal, 
            IIntegrationDal integrationDal, 
            IDateTimeService dateTimeService)
        {
            _mapper = mapper;
            _tvMazeClient = tvMazeClient;
            _presentationDal = presentationDal;
            _integrationDal = integrationDal;
            _dateTimeService = dateTimeService;
        }
        
        public async Task CreateOrUpdateTvShowAsync(int tvShowId, CancellationToken cancellationToken)
        {
            var tvShowExternalModel = await _tvMazeClient.GetTvShowInfoAsync(tvShowId, cancellationToken);

            if (tvShowExternalModel == null)
            {
                throw new EntityNotFoundException($"Tv show with id: {tvShowId} was not found at TvMaze api side.");
            }

            var tvShow = _mapper.Map<TvShow>(tvShowExternalModel);

            await _presentationDal.InsertOrUpdateItemAsync(tvShow, cancellationToken);
        }

        public async Task<IList<int>> GetOutdatedTvShowInfosIdsAsync(CancellationToken cancellationToken)
        {
            var updateResultTask =  _tvMazeClient.GetUpdatesAsync(cancellationToken);

            var recentHistoryRecord = await _integrationDal.GetRecentHistoryRecordAsync(cancellationToken);

            var updateResult = await updateResultTask;

            if (recentHistoryRecord == null)
            {
                return updateResult.Keys.ToList();
            }

            var integrationTaskStartTimestamp = _dateTimeService.ConvertToUnixTimestamp(recentHistoryRecord.UpdateProcessStartDate);

            var keysToUpdate = updateResult.Where(ur => ur.Value > integrationTaskStartTimestamp).Select(ur => ur.Key);

            return keysToUpdate.ToList();
        }
    }
}