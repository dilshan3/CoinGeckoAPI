using CoinGeckoAPI.Models;
using CoinGeckoAPI.Repositories;

namespace CoinGeckoAPI.Services
{
    public class CoinService
    {
        private readonly HttpClient _httpClient;
        private readonly CoinRepository _repository;
        private readonly string _baseUrl;

        public CoinService(HttpClient httpClient, CoinRepository repository, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _repository = repository;
            _baseUrl = configuration.GetValue<string>("CoinGeckoApi:BaseUrl") ?? "https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd";
        }

        // Get Coin by Id from CoinGecko API and save to DB if not found in DB
        public async Task<Coin?> GetCoinAsync(string id)
        {
            try
            {
                var cachedCoin = await _repository.GetByIdAsync(id);
                if (cachedCoin != null)
                    return cachedCoin;

                var response = await _httpClient.GetAsync($"{_baseUrl}&ids={id.ToLower()}");
                if (response.IsSuccessStatusCode)
                {
                    var coinList = await response.Content.ReadFromJsonAsync<List<Coin>>();
                    var coin = coinList?.FirstOrDefault();

                    if (coin != null)
                    {
                        await _repository.AddAsync(coin);
                        return coin;
                    }
                }
                return null;
            }
            catch (HttpRequestException hx)
            {
                throw new Exception("Failed to fetch data from CoinGecko API.", hx);
            }
            catch (Exception ex)
            {
                throw new Exception("Service failed to retrieve or save coin data.", ex);
            }
        }

        public async Task<List<Coin>> GetAllAsync()
        {
            try
            {
                var coins = await _repository.GetAllAsync();
                if (coins.Any()) return coins;

                var response = await _httpClient.GetAsync(_baseUrl + "&ids=bitcoin,ethereum");
                if (response.IsSuccessStatusCode)
                {
                    var coinList = await response.Content.ReadFromJsonAsync<List<Coin>>();
                    foreach (var coin in coinList ?? new List<Coin>())
                    {
                        await _repository.AddAsync(coin);
                    }
                    return coinList ?? new List<Coin>();
                }

                return new List<Coin>();
            }
            catch (HttpRequestException hx)
            {
                throw new Exception("Failed to fetch data from CoinGecko API.", hx);
            }
            catch (Exception ex)
            {
                throw new Exception("Service failed to retrieve or save coins data.", ex);
            }
        }
    }
}
