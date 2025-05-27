using CoinGeckoAPI.Models;
using Microsoft.Data.SqlClient;

namespace CoinGeckoAPI.Repositories
{
    public class CoinRepository
    {
        private readonly string _connectionString;

        //GenAI
        public CoinRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Get Coin by Id from DB
        public async Task<Coin?> GetByIdAsync(string id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                var cmd = new SqlCommand("SELECT * FROM Coins WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new Coin
                    {
                        Id = reader.GetString(0),
                        Symbol = reader.GetString(1),
                        Name = reader.GetString(2),
                        CurrentPrice = reader.GetDecimal(3),
                        MarketCap = reader.GetInt64(4)
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve coin with Id {id} from DB due to: " + ex.Message, ex);
            }
        }

        // Get all Coins from DB
        public async Task<List<Coin>> GetAllAsync()
        {
            try
            {
                var list = new List<Coin>();
                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                var cmd = new SqlCommand("SELECT * FROM Coins", conn);
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new Coin
                    {
                        Id = reader.GetString(0),
                        Symbol = reader.GetString(1),
                        Name = reader.GetString(2),
                        CurrentPrice = reader.GetDecimal(3),
                        MarketCap = reader.GetInt64(4)
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve coins from the database due to: " + ex.Message, ex);
            }
        }

        // Add new Coin to DB
        public async Task AddAsync(Coin coin)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                var cmd = new SqlCommand("INSERT INTO Coins (Id, Symbol, Name, CurrentPrice, MarketCap) VALUES (@Id, @Symbol, @Name, @CurrentPrice, @MarketCap)", conn);
                cmd.Parameters.AddWithValue("@Id", coin.Id);
                cmd.Parameters.AddWithValue("@Symbol", coin.Symbol);
                cmd.Parameters.AddWithValue("@Name", coin.Name);
                cmd.Parameters.AddWithValue("@CurrentPrice", coin.CurrentPrice);
                cmd.Parameters.AddWithValue("@MarketCap", coin.MarketCap);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to insert coin into the database due to: " + ex.Message, ex);
            }
        }
    }
}
