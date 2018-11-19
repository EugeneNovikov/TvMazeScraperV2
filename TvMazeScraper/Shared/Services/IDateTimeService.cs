using System;

namespace Shared.Services
{
    public interface IDateTimeService
    {
        DateTime UtcNow { get; }

        long ConvertToUnixTimestamp(DateTime dateTime);

        DateTime ConvertFromUnixTimestamp(long timestamp);
    }
}