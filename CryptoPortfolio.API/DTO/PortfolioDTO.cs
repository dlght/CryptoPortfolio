using CryptoPortfolio.API.Helpers;

namespace CryptoPortfolio.API.DTO
{
    public class PortfolioDTO
    {
        public long PortfolioId { get; set; }

        public decimal InitialTotal { get; set; }

        public decimal TotalValue { get; set; }

        public List<CurrencyDTO> Currencies { get; set; }

        public decimal ChangeTotalInPercentage 
        { 
            get 
            {
                return Extensions.CalculatePercentageChange(this.TotalValue, this.InitialTotal);
            } 
        }
    }
}
