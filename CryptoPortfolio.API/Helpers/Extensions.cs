namespace CryptoPortfolio.API.Helpers
{
    public static class Extensions
    {
        public static decimal CalculatePercentageChange(decimal currentPrice, decimal initialPrice)
        {
            if (initialPrice == 0)
            {
                throw new ArgumentException("Initial price cannot be zero.");
            }

            decimal difference = currentPrice - initialPrice;
            decimal percentageChange = difference / initialPrice * 100;

            return decimal.Parse(percentageChange.ToString());
        }
    }
}
