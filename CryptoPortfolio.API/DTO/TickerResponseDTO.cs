using Newtonsoft.Json;

namespace CryptoPortfolio.API.DTO
{
    public class TickerResponseDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nameid")]
        public string NameId { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("price_usd")]
        public string PriceUsd { get; set; }

        [JsonProperty("percent_change_24h")]
        public string PercentChange24h { get; set; }

        [JsonProperty("percent_change_1h")]
        public string PercentChange1h { get; set; }

        [JsonProperty("percent_change_7d")]
        public string PercentChange7d { get; set; }

        [JsonProperty("price_btc")]
        public string PriceBtc { get; set; }

        [JsonProperty("market_cap_usd")]
        public string MarketCapUsd { get; set; }

        [JsonProperty("volume24")]
        public double Volume24 { get; set; }

        [JsonProperty("volume24a")]
        public double Volume24a { get; set; }

        [JsonProperty("csupply")]
        public string Csupply { get; set; }

        [JsonProperty("tsupply")]
        public string Tsupply { get; set; }

        [JsonProperty("msupply")]
        public string Msupply { get; set; }
    }
}