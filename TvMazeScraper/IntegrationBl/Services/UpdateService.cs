using System.Threading;
using System.Threading.Tasks;
using IntegrationBl.Factories;
using Microsoft.Extensions.Logging;
using Shared.Extensions;
using Shared.Interfaces;
using Shared.Models.Integration;
using Shared.Services;

namespace IntegrationBl.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly IIntegrationDal _integrationDal;
        private readonly IDateTimeService _dateTimeService;
        private readonly ITvShowUpdateService _tvShowUpdateService;
        private readonly IWorkloadService _workloadService;
        private readonly IPolicyFactory _policyFactory;
        private readonly ILogger<UpdateService> _logger;

        public UpdateService(IIntegrationDal integrationDal, 
            IDateTimeService dateTimeService, 
        
            ITvShowUpdateService tvShowUpdateService,
            IWorkloadService workloadService, 
            IPolicyFactory policyFactory, 
            ILogger<UpdateService> logger)
        {
            _integrationDal = integrationDal;
            _dateTimeService = dateTimeService;
            _tvShowUpdateService = tvShowUpdateService;
            _workloadService = workloadService;
            _policyFactory = policyFactory;
            _logger = logger;
        }

        public async Task StartUpdateProcessAsync(CancellationToken cancellationToken)
        {
            var tvShowsToUpdateIds = await _tvShowUpdateService.GetOutdatedTvShowInfosIdsAsync(cancellationToken);

            if (tvShowsToUpdateIds.IsNullOrEmpty())
            {
                return;
            }

            _logger.LogInformation($"{tvShowsToUpdateIds.Count} TV shows to update found. Update process started.");

            var historyRecord = new HistoryRecord(_dateTimeService.UtcNow);
            
            var policies = _policyFactory.CreateUpdateTaskPolicies(() => _workloadService.IncreaseDelayTime());
            
            foreach (var tvShowToUpdateId in tvShowsToUpdateIds)
            {
                await Task.Delay(_workloadService.UpdateTvShowInfoTaskExecutionDelay, cancellationToken);
                
                await policies.ExecuteAsync( () =>
                     _tvShowUpdateService.CreateOrUpdateTvShowAsync(tvShowToUpdateId, cancellationToken));
            }

            await _integrationDal.SaveHistoryRecordAsync(historyRecord, cancellationToken);
        }
    }
}
