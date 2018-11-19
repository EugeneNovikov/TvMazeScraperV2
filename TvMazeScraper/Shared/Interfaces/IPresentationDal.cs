using System.Threading;
using System.Threading.Tasks;
using Shared.Models.Paging;
using Shared.Models.Presentation;

namespace Shared.Interfaces
{
    public interface IPresentationDal
    {
        Task InsertOrUpdateItemAsync(TvShow tvShow, CancellationToken cancellationToken);

        Task<PaginatedResult<TvShow>> GetTvShowsWithActorsSortedDescendingAsync(TvShowRequest tvShowRequest, CancellationToken cancellationToken);
    }
}