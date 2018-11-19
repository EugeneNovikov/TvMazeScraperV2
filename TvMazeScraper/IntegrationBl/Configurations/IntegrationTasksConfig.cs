using System;

namespace IntegrationBl.Configurations
{
    public class IntegrationTasksConfig
    {
        public TimeSpan UpdateInfoAboutTvShowExecutionDelay { get; set; }

        public TimeSpan MinimalIntervalBetweenDelayIncreasing { get; set; }

        public int IncreaseDelayStepMilliseconds { get; set; }
        
        public TimeSpan StartUpdateProcessTaskExecutionDelay { get; set; }

    }
}
