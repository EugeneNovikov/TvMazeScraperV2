using System.Threading;
using System.Threading.Tasks;
using Shared.Models.Paging;
using Shared.Models.Presentation;

namespace PresentationBl.Services
{
    public interface IPresentationService
    {
        Task<PaginatedResult<TvShow>> GetTvShowsWithActorsSortedDescendingAsync(TvShowRequest request, CancellationToken cancellationToken);
    }
}