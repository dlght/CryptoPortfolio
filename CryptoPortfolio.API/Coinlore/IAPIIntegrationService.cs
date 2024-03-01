using CryptoPortfolio.API.Coinlore.DTO;
using CryptoPortfolio.API.Coinlore.Enums;

namespace CryptoPortfolio.API.Coinlore
{
    public interface IAPIIntegrationService
    {
        Task<TickerResponseDTO> GetTicker(CoinloreCryptoCurrenciesEnum currency);
    }
}
