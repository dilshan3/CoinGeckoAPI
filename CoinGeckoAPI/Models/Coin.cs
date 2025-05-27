using System.Text.Json.Serialization;

namespace CoinGeckoAPI.Models
{
    public class Coin
    {
        public string Id { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("current_price")]
        public decimal CurrentPrice { get; set; }
        [JsonPropertyName("market_cap")]
        public long MarketCap { get; set; }
    }

}
