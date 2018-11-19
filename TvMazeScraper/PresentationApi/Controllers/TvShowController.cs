using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PresentationApi.Configurations;
using PresentationBl.Services;
using Shared.Models.Paging;
using Shared.Models.Presentation;

namespace PresentationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TvShowController : ControllerBase
    {
        private readonly IPresentationService _presentationService;
        private readonly ApplicationSettings _applicationSettings;

        public TvShowController(IPresentationService presentationService, 
            IOptions<ApplicationSettings> applicationSettings)
        {
            _presentationService = presentationService;
            _applicationSettings = applicationSettings?.Value ?? throw new ArgumentNullException(nameof(applicationSettings));
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TvShow>>> GetAsync (string show, int? pagesize, int? pageNumber, CancellationToken cancellationToken)
        {
            var tvShowRequest = new TvShowRequest
            {
                Name = show,
                PageSize = pagesize ?? _applicationSettings.DefaultPageSize,
                PageNumber = pageNumber ?? 0
            };
            
            var paginatedResult = await _presentationService.GetTvShowsWithActorsSortedDescendingAsync(tvShowRequest, cancellationToken);

            if (paginatedResult?.Items == null)
            {
                return NotFound();
            }

            return Ok(paginatedResult.Items);
        }
    }
}
