using CryptoPortfolio.API.Helpers;

namespace CryptoPortfolio.API.DTO
{
    public class CurrencyDTO
    {
        public long CurrencyId { get; set; }

        public string CurrencyCode { get; set; }

        public decimal InitialValue { get; set; }

        public decimal NumberOfCoins { get; set; }

        public decimal CurrentValue { get; set; }

        public decimal ChangeInPercent 
        { 
            get 
            {
                return Extensions.CalculatePercentageChange(this.CurrentValue, this.InitialValue);
            } 
        }
    }
}
