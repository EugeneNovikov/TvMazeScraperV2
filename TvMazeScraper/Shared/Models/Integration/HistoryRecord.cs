using System;

namespace Shared.Models.Integration
{
    public class HistoryRecord
    {
        public HistoryRecord(DateTime updateProcessStartDate)
        {
            UpdateProcessStartDate = updateProcessStartDate;
        }

        public DateTime UpdateProcessStartDate { get; }
    }
}
