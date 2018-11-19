using System;
using Polly.Wrap;

namespace IntegrationBl.Factories
{
    public interface IPolicyFactory
    {
        PolicyWrap CreateUpdateTaskPolicies(Action onRetry);
    }
}