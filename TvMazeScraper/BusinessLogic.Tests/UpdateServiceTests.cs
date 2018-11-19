using System;
using System.Collections.Generic;
using System.Threading;
using IntegrationBl.Factories;
using IntegrationBl.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Polly;
using Polly.Wrap;
using Shared.Interfaces;
using Shared.Models.Integration;
using Shared.Services;
using Xunit;

namespace BusinessLogic.Tests
{
    public class UpdateServiceTests
    {
        private readonly IUpdateService _updateService;

        private readonly Mock<IIntegrationDal> _integrationDal;

        private readonly Mock<ITvShowUpdateService> _tvShowUpdateService;

        private readonly Mock<IPolicyFactory> _policyFactory;

        private readonly Mock<IWorkloadService> _workloadService;

        private readonly CancellationToken _cancellationToken;

        private readonly Mock<ILogger<UpdateService>> _logger;

        private readonly PolicyWrap _policyWrapStub;

        public UpdateServiceTests()
        {
            _integrationDal = new Mock<IIntegrationDal>();

            var dateTimeService = new Mock<IDateTimeService>();

            _tvShowUpdateService = new Mock<ITvShowUpdateService>();

            _policyFactory = new Mock<IPolicyFactory>();

            _workloadService = new Mock<IWorkloadService>();

            _logger = new Mock<ILogger<UpdateService>>();

            var retryPolicy = Policy.Handle<Exception>().RetryAsync(3);
            var circuitBreakerPolicy = Policy.Handle<Exception>()
                .CircuitBreakerAsync(1, TimeSpan.FromSeconds(1));

            _policyWrapStub = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);

            _cancellationToken = new CancellationToken();

            _policyFactory.Setup(f => f.CreateUpdateTaskPolicies(It.IsAny<Action>())).Returns(() => _policyWrapStub);

            _updateService = new UpdateService(
                _integrationDal.Object,
                dateTimeService.Object,
                _tvShowUpdateService.Object,
                _workloadService.Object,
                _policyFactory.Object,
                _logger.Object
            );
        }

        [Fact]
        public async void Processing_Not_Started_If_Nothing_To_Process()
        {
            _tvShowUpdateService.Setup(t => t.GetOutdatedTvShowInfosIdsAsync(_cancellationToken))
                .ReturnsAsync(() => null);

            await _updateService.StartUpdateProcessAsync(_cancellationToken);

            _policyFactory.Verify(f => f.CreateUpdateTaskPolicies(It.IsAny<Action>()), Times.Never);
        }

        [Fact]
        public async void Verify_New_History_Record_Created_At_Process_End()
        {
            SetupTvShowsToBeUpdated();

            await _updateService.StartUpdateProcessAsync(_cancellationToken);

            _integrationDal.Verify(i => i.SaveHistoryRecordAsync(It.IsAny<HistoryRecord>(), _cancellationToken),
                Times.Once);
        }

        [Fact]
        public async void Verify_Update_Tv_Show_Service_Called_For_Every_Item()
        {
            SetupTvShowsToBeUpdated();

            await _updateService.StartUpdateProcessAsync(_cancellationToken);

            _tvShowUpdateService.Verify(t => t.CreateOrUpdateTvShowAsync(It.IsAny<int>(), _cancellationToken),
                Times.Exactly(3));
        }


        private void SetupTvShowsToBeUpdated()
        {
            var listOfOutdatedShowInfos = new List<int> {1, 2, 3};

            _tvShowUpdateService.Setup(t => t.GetOutdatedTvShowInfosIdsAsync(_cancellationToken))
                .ReturnsAsync(listOfOutdatedShowInfos);
        }
    }
}