using System;

namespace IntegrationBl.Services
{
    public interface IWorkloadService
    {
        TimeSpan UpdateTvShowInfoTaskExecutionDelay { get; }

        void IncreaseDelayTime();
    }
}