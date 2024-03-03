using CryptoPortfolio.API.Coinlore;
using CryptoPortfolio.API.Coinlore.Enums;
using CryptoPortfolio.API.DTO;
using Microsoft.Extensions.Caching.Memory;

namespace CryptoPortfolio.API.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IAPIIntegrationService _apiIntegration;

        private readonly IMemoryCache _cache;
        private readonly ILogger<ApplicationService> _logger;
        public ApplicationService(
            IAPIIntegrationService apiIntegration,
            IMemoryCache cache,
            ILogger<ApplicationService> logger) 
        {
            _apiIntegration = apiIntegration;
            _cache = cache;
            _logger = logger;
        }

        public async Task<long> ImportPortfolio(FileUploadDTO fileUpload)
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
                            var holding = this.ParseRow(line, index);
                            _logger.LogCritical($"Add holding's parsed data to the current portfolio for currency {holding.CurrencyCode}");

                            portfolio.Currencies.Add(holding);

                            index++;
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                // Handle file access errors
                throw new InvalidOperationException($"Error reading file: {fileUpload.File.FileName}, Exception: {ex.Message}");
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

        public async Task<PortfolioDTO?> RefreshPortfolio(long portfolioId)
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

        private CurrencyDTO ParseRow(string line, int index)
        {
            try
            {
                var parts = line.Split('|');
                if (parts.Length != 3)
                {
                    throw new ArgumentException($"Invalid line format: {line}");
                }

                var holding = new CurrencyDTO
                {
                    CurrencyId = index,
                    NumberOfCoins = decimal.Parse(parts[0]),
                    CurrencyCode = parts[1],
                    InitialValue = decimal.Parse(parts[2]),
                    CurrentValue = decimal.Parse(parts[2])
                };

                return holding;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    throw;
                }
                else
                {
                    // Log or handle parsing errors for specific lines
                    throw new ArgumentException($"Cannot parse current row: {line}");
                }
            }
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
