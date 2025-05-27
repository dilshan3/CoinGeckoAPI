# CoinGeckoAPI

A lightweight ASP.NET Core Web API project that fetches cryptocurrency data from the CoinGecko public API and caches it in a SQL Server database using raw SQL.

## ✅ Features

- 🔗 **Integration with CoinGecko Public API** (no key required)
- 🗃️ **SQL Server data persistence** via `Microsoft.Data.SqlClient`
---

## 📌 Endpoints

| Method | Endpoint            | Description |
|--------|---------------------|-------------|
| GET    | `/api/coin`         | Returns all coin records from the database |
| GET    | `/api/coin/{id}`    | Returns a coin by ID (e.g., `bitcoin`). Fetches from API if not in DB |

---

## 🛠️ Setup Instructions

### 🔧 Prerequisites
- .NET SDK 8
- SQL Server (local)

### ⚙️ SQL Setup
Run the following to create the required table:
```sql
CREATE TABLE Coins (
    Id NVARCHAR(100) PRIMARY KEY,
    Symbol NVARCHAR(20),
    Name NVARCHAR(100),
    CurrentPrice DECIMAL(18, 8),
    MarketCap BIGINT
);
```

### Run App on Command Prompt
- git clone https://github.com/dilshan3/CoinGeckoAPI
- cd CoinGeckoAPI
- dotnet restore
- dotnet run --launch-profile https --project CoinGeckoAPI/CoinGeckoAPI.csproj

### Test App
GET https://localhost:7255/api/coin/bitcoin

---

### 📌 Design Decisions
- Seeded Coins Only: When the DB is empty, the service seeds selected coins (e.g., Bitcoin, Ethereum) when the `/api/coin` is called. This avoids loading thousands of records unnecessarily.
- Microsoft.Data.SqlClient: This NuGet package was used instead of System.Data.SqlClient for compatibility with modern .NET (Core/8+), better performance, and support for modern SQL Server features.

