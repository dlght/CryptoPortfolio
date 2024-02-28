using CryptoPortfolio.API.Coinlore;
using CryptoPortfolio.API.Controllers;
using CryptoPortfolio.API.DTO;
using CryptoPortfolio.API.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace CryptoPortfolio.API.Services
{
    public class ApplicationService
    {
        private readonly APIIntegrationService _apiIntegration;

        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApplicationService> _logger;
        public ApplicationService(
            APIIntegrationService apiIntegration,
            IMemoryCache cache,
            IConfiguration configuration,
            ILogger<ApplicationService> logger) 
        {
            _apiIntegration = apiIntegration;
            _cache = cache;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<PortfolioDTO?> GetUpdatePortfolio(long portfolioId)
        {
            _logger.LogCritical("Try to get the portfolio");
            var cachedPortfolio = this.GetFromCache(portfolioId);
            _logger.LogCritical("Successfully got the portfolio from cache");

            decimal newTotal = 0m;
            foreach (var currency in cachedPortfolio.Currencies)
            {
                _logger.LogCritical($"Try to get the changes for currency: {currency.CurrencyCode}");
                var currencyEnum = Enum.Parse<CoinloreCryptoCurrenciesEnum>(currency.CurrencyCode, true);
                var currencyTickerData = await this._apiIntegration.GetTicker(currencyEnum);
                _logger.LogCritical($"Successfully got the changes for currency: {currency.CurrencyCode}");

                decimal newValue = decimal.Parse(currencyTickerData.PriceUsd);
                _logger.LogCritical($"Update the price for: {currency.CurrencyCode}. Old value: {currency.CurrentValue} USD, New value: {newValue} USD");
                currency.CurrentValue = newValue;

                newTotal += currency.CurrentValue * currency.NumberOfCoins;
            }

            _logger.LogCritical($"Update the current total portfolio total. Old value: {cachedPortfolio.TotalValue} USD, New value: {newTotal} USD");
            cachedPortfolio.TotalValue = newTotal;

            _logger.LogCritical($"Update the record for the portfolio with all the updates on the data.");
            this.UpdateInCache(cachedPortfolio);

            return cachedPortfolio;
        }

        public async Task<long> CreatePortfolio(FileUploadDTO fileUpload)
        {
            var portfolio = new PortfolioDTO()
            {
                PortfolioId = 1,
                Currencies = new List<CurrencyDTO>()
            };

            _logger.LogCritical($"Parse the file with the currency values and prices.");
            try
            {
                using (var stream = fileUpload.File.OpenReadStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        string line;
                        int index = 1;
                        while ((line = await reader.ReadLineAsync()) != null)
                        {
                            try
                            {
                                var parts = line.Split('|');
                                if (parts.Length != 3)
                                {
                                    throw new Exception($"Invalid line format: {line}");
                                }

                                var holding = new CurrencyDTO
                                {
                                    CurrencyId = index,
                                    NumberOfCoins = decimal.Parse(parts[0]),
                                    CurrencyCode = parts[1],
                                    InitialValue = decimal.Parse(parts[2]),
                                    CurrentValue = decimal.Parse(parts[2])
                                };

                                _logger.LogCritical($"Add holding's parsed data to the current portfolio for currency {holding.CurrencyCode}");
                                portfolio.Currencies.Add(holding);
                            }
                            catch (Exception ex)
                            {
                                // Log or handle parsing errors for specific lines
                                Console.WriteLine($"Error parsing line: {line}, Exception: {ex.Message}");
                            }

                            index++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle file access errors
                Console.WriteLine($"Error reading file: {fileUpload.File.FileName}, Exception: {ex.Message}");
            }

            decimal total = 0m;

            _logger.LogCritical($"Calculate current portfolio initial value in USD.");
            foreach (var currency in portfolio.Currencies)
            {
                total += currency.NumberOfCoins * currency.CurrentValue;
            }

            portfolio.InitialTotal = total;
            portfolio.TotalValue = total;

            _logger.LogCritical($"Add current portfolio record to cache.");
            this.AddInCache(portfolio);

            return portfolio.PortfolioId;
        }

        #region small in memory DAL

        private void AddInCache(PortfolioDTO dto)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1000));

            _cache.Set(dto.PortfolioId, dto, cacheEntryOptions);
        }

        private void UpdateInCache(PortfolioDTO dto)
        {
            _cache.Remove(dto.PortfolioId);

            this.AddInCache(dto);
        }

        private PortfolioDTO GetFromCache(long key)
        {
            var cachedPortfolio = _cache.Get<PortfolioDTO>(key);

            if (cachedPortfolio == null)
            {
                throw new InvalidOperationException("Portfolio not found");
            }

            return cachedPortfolio;
        }

        #endregion

    }
}
