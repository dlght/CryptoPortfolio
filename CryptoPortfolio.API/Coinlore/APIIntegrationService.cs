using System.Globalization;
using CryptoPortfolio.API.Controllers;
using CryptoPortfolio.API.DTO;
using CryptoPortfolio.API.Enums;
using Newtonsoft.Json;

namespace CryptoPortfolio.API.Coinlore
{
    public class APIIntegrationService
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
                var baseAddress = _configuration["CoinloreConfig:BaseAddress"];
                var tickerEndpoint = _configuration["CoinloreConfig:Endpoints:Ticker"];

                string url = $"{baseAddress}{tickerEndpoint}/?id=";

                var requestUrl = url + (int)currency;

                HttpResponseMessage response = await _httpClientFactory.CreateClient().GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
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