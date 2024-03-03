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
        private readonly string _coinloreBaseUrl;

        public APIIntegrationService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<APIIntegrationService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
            _coinloreBaseUrl = _configuration["CoinloreConfig:BaseAddress"];
        }

        public async Task<TickerResponseDTO> GetTicker(CoinloreCryptoCurrenciesEnum currency)
        {
            _logger.LogCritical($"Trying to refresh data from coinlore/ticker for currency: {currency}");

            try
            {
                var tickerEndpoint = _configuration["CoinloreConfig:Endpoints:Ticker"];

                string _tickerEndpointUrl = $"{_coinloreBaseUrl}{tickerEndpoint}/?id=";
                var requestUrl = _tickerEndpointUrl + (int)currency;

                _logger.LogCritical($"Request ticker with url: {requestUrl}");

                HttpResponseMessage response = await _httpClientFactory.CreateClient().GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogCritical($"Successful response from coinlore/ticker for currency: {currency}");

                    string responseContent = await response.Content.ReadAsStringAsync();

                    var settings = new JsonSerializerSettings { Culture = CultureInfo.InvariantCulture };
                    var tickerResult = JsonConvert.DeserializeObject<List<TickerResponseDTO>>(responseContent);

                    // return the first result -- according to the documentation if its not a list of 1 it should return exception
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