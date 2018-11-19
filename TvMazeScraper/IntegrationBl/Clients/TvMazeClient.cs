using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IntegrationBl.Configurations;
using Microsoft.Extensions.Options;
using Shared.Extensions;
using Shared.Models.Communication;
using Shared.Services;
using Shared.Wrappers;

namespace IntegrationBl.Clients
{
    public class TvMazeClient : ITvMazeClient
    {
        private readonly HttpClient _httpClient;
        private readonly IJsonConverterWrapper _jsonConverter;
        private readonly TvMazeConfig _tvMazeConfig;
        private readonly IUrlFormatService _urlFormatService;

        public TvMazeClient(HttpClient httpClient, 
            IOptions<TvMazeConfig> tvMazeConfig, 
            IJsonConverterWrapper jsonConverter, 
            IUrlFormatService urlFormatService)
        {
            _tvMazeConfig = tvMazeConfig.Value;

            _httpClient = httpClient;
            _jsonConverter = jsonConverter;
            _urlFormatService = urlFormatService;
            _httpClient.BaseAddress = new Uri(_tvMazeConfig.BaseAddress);
        }

        public async Task<IDictionary<int, int>> GetUpdatesAsync(CancellationToken cancellationToken)
        {
            var httpResponseMessage =  await _httpClient.GetAsync(_urlFormatService.FormatUrlComponent(_tvMazeConfig.UpdatesUrl), cancellationToken);

            var apiResponse = await httpResponseMessage.UnwindHttpExceptions().ReadContentAsStringAsync();

            return _jsonConverter.DeserializeObject<IDictionary<int,int>>(apiResponse);
        }

        public async Task<TvShowInfo> GetTvShowInfoAsync(int tvShowId, CancellationToken cancellationToken)
        {
            var httpResponseMessage = await _httpClient.GetAsync(_urlFormatService.FormatUrlComponent(_tvMazeConfig.ShowWithCastUrlTemplate, tvShowId), cancellationToken);

            var apiResponse = await httpResponseMessage.UnwindHttpExceptions().ReadContentAsStringAsync();
            
            return _jsonConverter.DeserializeObjectSafe<TvShowInfo>(apiResponse);
        }
    }
}
