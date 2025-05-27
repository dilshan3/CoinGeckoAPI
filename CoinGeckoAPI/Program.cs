using CoinGeckoAPI.Repositories;
using CoinGeckoAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<CoinRepository>();
//GenAI
builder.Services.AddHttpClient<CoinService>(client =>
{
    client.DefaultRequestHeaders.Add("User-Agent", "CoinGeckoAPIClient/1.0");
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();