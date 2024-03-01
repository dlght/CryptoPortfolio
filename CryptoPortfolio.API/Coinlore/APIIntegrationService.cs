using System.Globalization;
using CryptoPortfolio.API.Coinlore.DTO;
using CryptoPortfolio.API.Coinlore.Enums;
using Newtonsoft.Json;

namespace CryptoPortfolio.API.Coinlore
{
    public class APIIntegrationService : IAPIIntegrationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<APIIntegrationService> _logger;
        private readonly IConfiguration _configuration;

        public APIIntegrationService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<APIIntegrationService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<TickerResponseDTO> GetTicker(CoinloreCryptoCurrenciesEnum currency)
        {
            try
            {
                _logger.LogCritical($"Trying to refresh data from coinlore/ticker for currency: {currency}");

                var baseAddress = _configuration["CoinloreConfig:BaseAddress"];
                var tickerEndpoint = _configuration["CoinloreConfig:Endpoints:Ticker"];

                string url = $"{baseAddress}{tickerEndpoint}/?id=";

                var requestUrl = url + (int)currency;

                _logger.LogCritical($"Request ticker with url: {requestUrl}");

                HttpResponseMessage response = await _httpClientFactory.CreateClient().GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogCritical($"Successful response from coinlore/ticker for currency: {currency}");

                    string responseContent = await response.Content.ReadAsStringAsync();

                    var settings = new JsonSerializerSettings { Culture = CultureInfo.InvariantCulture };
                    var tickerResult = JsonConvert.DeserializeObject<List<TickerResponseDTO>>(responseContent);
                    return tickerResult.First();
                }
                else
                {
                    throw new Exception($"Error calling API: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving users: {ex.Message}");
            }
        }
    }
}