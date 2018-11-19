using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IntegrationBl.Services
{
    public interface ITvShowUpdateService
    {
        Task CreateOrUpdateTvShowAsync(int tvShowId, CancellationToken cancellationToken);

        Task<IList<int>> GetOutdatedTvShowInfosIdsAsync(CancellationToken cancellationToken);
    }
}