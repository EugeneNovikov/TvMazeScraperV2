using System;
using IntegrationBl.Configurations;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Wrap;
using Shared.Exceptions;

namespace IntegrationBl.Factories
{
    public class PolicyFactory : IPolicyFactory
    {
        private readonly PoliciesConfig _policiesConfig;
        
        public PolicyFactory(IOptions<PoliciesConfig> policiesConfig)
        {
            _policiesConfig = policiesConfig?.Value ?? throw new ArgumentNullException(nameof(PoliciesConfig));
        }

        public PolicyWrap CreateUpdateTaskPolicies(Action onRetry)
        {
            return Policy.WrapAsync(CreateRetryPolicyWithExponentialBackoff(onRetry), CreateCircuitBreakerPolicy());
        }

        private Policy CreateRetryPolicyWithExponentialBackoff(Action onRetry)
        {
            return Policy.Handle<TooManyRequestsException>()
                .WaitAndRetryAsync(_policiesConfig.RetryCount,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) => {
                        onRetry();
                    });
        }

        private Policy CreateCircuitBreakerPolicy()
        {
            return Policy.Handle<InternalServerErrorException>()
                .CircuitBreakerAsync(_policiesConfig.RetryCount, _policiesConfig.ExternalServerFailureRetryTimeout);
        }
    }
}