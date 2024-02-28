namespace CryptoPortfolio.API.Helpers
{
    public static class Extensions
    {
        public static string CalculatePercentageChange(decimal currentPrice, decimal initialPrice)
        {
            if (initialPrice == 0)
            {
                throw new ArgumentException("Initial price cannot be zero.");
            }

            decimal difference = currentPrice - initialPrice;
            decimal percentageChange = difference / initialPrice * 100;

            // Format the result with two decimal places and "%" symbol
            return decimal.Parse(percentageChange.ToString()).ToString("0.00") + "%";
        }
    }
}
