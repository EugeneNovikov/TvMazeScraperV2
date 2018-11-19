using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shared.Models.Communication;

namespace IntegrationBl.Clients
{
    public interface ITvMazeClient
    {
        Task<IDictionary<int, int>> GetUpdatesAsync(CancellationToken cancellationToken);

        Task<TvShowInfo> GetTvShowInfoAsync(int tvShowId, CancellationToken cancellationToken);
    }
}