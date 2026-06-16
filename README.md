# FinTrack API

A REST API for personal finance management built with ASP.NET Core, Entity Framework Core, and PostgreSQL.

---

## About

FinTrack API allows users to manage their personal finances by registering income and expense transactions, organizing them by category, and consulting financial summaries by period.

---

## Technologies

- **[.NET 10](https://dotnet.microsoft.com/)** — Framework
- **[ASP.NET Core Web API](https://learn.microsoft.com/en-us/aspnet/core/)** — REST API
- **[Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)** — ORM
- **[PostgreSQL](https://www.postgresql.org/)** — Database
- **[JWT Bearer](https://jwt.io/)** — Authentication
- **[BCrypt.Net](https://github.com/BcryptNet/bcrypt.net)** — Password hashing
- **[FluentValidation](https://docs.fluentvalidation.net/)** — Input validation
- **[Scalar](https://scalar.com/)** — API documentation

---

## Features

- User registration and login with JWT authentication
- Password encryption with BCrypt
- Full CRUD for transactions and categories
- Date range filtering for transactions
- Financial summary by period (total income, total expense, balance)
- Input validation with FluentValidation
- Global error handling middleware

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL 16+](https://www.postgresql.org/download/)

### Setup

**1. Clone the repository**
```bash
git clone https://github.com/pedrobinelo/fintrack-api.git
cd fintrack-api
```

**2. Configure the database and JWT**

Copy `appsettings.Example.json` to `appsettings.json` and fill in your values:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=fintrack;Username=postgres;Password=YOUR_PASSWORD"
  },
  "Jwt": {
    "Key": "your-secret-key-with-at-least-32-characters",
    "Issuer": "FinTrackAPI",
    "Audience": "FinTrackAPI"
  }
}
```

**3. Apply migrations**

Make sure you have the Entity Framework tool installed globally:

```bash
dotnet tool install --global dotnet-ef
```

Next, run the command to create the tables in your PostgreSQL:

```bash
dotnet ef database update
```

**4. Run the project**
```bash
dotnet run
```

**5. Access the API documentation**

Open your browser to the URL indicated on your device.
```
http://localhost:XXXX/scalar/v1
```

---

## API Endpoints

### Authentication

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Login and receive JWT token |

### Transactions

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/transactions` | Create a transaction |
| GET | `/api/transactions` | List transactions (supports date filter) |
| GET | `/api/transactions/{id}` | Get transaction by ID |
| PUT | `/api/transactions/{id}` | Update a transaction |
| DELETE | `/api/transactions/{id}` | Delete a transaction |
| GET | `/api/transactions/summary` | Get financial summary by period |

### Categories

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/categories` | List categories |
| POST | `/api/categories` | Create a category |

---

## Filtering and Summary

Transactions and the summary endpoint support optional date range filtering via query parameters:

```
GET /api/transactions?startDate=2026-01-01&endDate=2026-06-30
GET /api/transactions/summary?startDate=2026-01-01&endDate=2026-06-30
```

### Summary response example

```json
{
  "totalIncome": 5000.00,
  "totalExpense": 2300.00,
  "balance": 2700.00,
  "startDate": "2026-01-01T00:00:00",
  "endDate": "2026-06-30T00:00:00"
}
```

---

## Project Structure

```
FinTrackAPI/
├── Controllers/       # API endpoints
├── Data/              # DbContext
├── DTOs/              # Data Transfer Objects
├── Migrations/        # EF Core migrations
├── Models/            # Domain entities
├── Repositories/      # Data access layer
├── Services/          # Business logic layer
└── Program.cs         # Application entry point
```

---

## Author

Pedro Binelo — [github.com/pedrobinelo](https://github.com/pedrobinelo)