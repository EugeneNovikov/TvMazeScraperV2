using System.Threading;
using System.Threading.Tasks;
using Shared.Interfaces;
using Shared.Models.Paging;
using Shared.Models.Presentation;

namespace PresentationBl.Services
{
    public class PresentationService : IPresentationService
    {
        private readonly IPresentationDal _presentationDal;
        
        public PresentationService(IPresentationDal presentationDal)
        {
            _presentationDal = presentationDal;
        }

        public async Task<PaginatedResult<TvShow>> GetTvShowsWithActorsSortedDescendingAsync(TvShowRequest request, CancellationToken cancellationToken)
        {
            return await _presentationDal.GetTvShowsWithActorsSortedDescendingAsync(request, cancellationToken);
        }
    }
}
