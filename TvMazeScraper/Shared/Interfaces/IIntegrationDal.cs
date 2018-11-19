using System.Threading;
using System.Threading.Tasks;
using Shared.Models.Integration;

namespace Shared.Interfaces
{
    public interface IIntegrationDal
    {
        Task<HistoryRecord> GetRecentHistoryRecordAsync(CancellationToken cancellationToken);

        Task SaveHistoryRecordAsync(HistoryRecord historyRecord, CancellationToken cancellationToken);
    }
}