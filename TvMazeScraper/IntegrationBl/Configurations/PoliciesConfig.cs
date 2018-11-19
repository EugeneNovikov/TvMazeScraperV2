using System;

namespace IntegrationBl.Configurations
{
    public class PoliciesConfig
    {
        public int RetryCount { get; set; }

        public TimeSpan ExternalServerFailureRetryTimeout { get; set; }
    }
}