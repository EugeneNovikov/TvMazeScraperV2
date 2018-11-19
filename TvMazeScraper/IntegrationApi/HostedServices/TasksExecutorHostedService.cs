using System;
using System.Threading;
using System.Threading.Tasks;
using IntegrationBl.Configurations;
using IntegrationBl.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IntegrationApi.HostedServices
{
    public class TasksExecutorHostedService : IHostedService
    {
        private readonly ILogger<TasksExecutorHostedService> _logger; 
        
        private readonly IntegrationTasksConfig _integrationTasksConfig;

        private readonly IUpdateService _updateService;
        
        public TasksExecutorHostedService(
            IOptions<IntegrationTasksConfig> integrationTasksConfig, 
            ILogger<TasksExecutorHostedService> logger, 
            IUpdateService updateService)
        {
            _logger = logger;

            _updateService = updateService;

            _integrationTasksConfig = integrationTasksConfig?.Value ?? throw new ArgumentNullException(nameof(IntegrationTasksConfig));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("Job started");
            
            while (true)
            {
                await StartUpdateProcessAsync(cancellationToken);

                await Task.Delay(_integrationTasksConfig.StartUpdateProcessTaskExecutionDelay, cancellationToken);
            }
        }
        
        public async Task StartUpdateProcessAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("Update process started");

            try
            {
                 await _updateService.StartUpdateProcessAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during starting updating process {ex.Message}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
