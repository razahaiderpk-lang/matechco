# General Ledger Service

A .NET Web API for a double-entry accounting ledger, integrated with **Neon PostgreSQL** and **Clerk Authentication**.

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Neon.tech Account](https://neon.tech) (for PostgreSQL)
- [Clerk.com Account](https://clerk.com) (for Authentication)

## Configuration

### 1. Database (Neon)
The application is pre-configured to use a Neon instance. The connection string is located in `GeneralLedgerService/appsettings.json`.

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=ep-quiet-bonus-ahvkijol.c-3.us-east-1.aws.neon.tech;Database=neondb;Username=neondb_owner;Password=***;Port=5432;SSL Mode=Require;Trust Server Certificate=true"
}
```

### 2. Authentication (Clerk)
The API is secured with Clerk JWT Authentication. The settings are in `appsettings.json`.

```json
"Clerk": {
  "Authority": "https://accepted-wallaby-49.clerk.accounts.dev",
  "Audience": "clerk"
}
```

## Running the Project

To run the project with the HTTPS profile (recommended for testing the Secure API), run the following command from the root directory:

```bash
dotnet run --project GeneralLedgerService --launch-profile https
```

The API will be available at:
- **HTTPS**: `https://localhost:7066`
- **HTTP**: `http://localhost:5135`

## Testing the API

### 1. Authorization Header
All endpoints (except for those marked otherwise) require a valid JWT. You must include the following header in your requests:

`Authorization: Bearer <YOUR_CLERK_TOKEN>`

### 2. Sample Endpoints
- **Get Accounts**: `GET https://localhost:7066/api/accounts`
- **Post Entry**: `POST https://localhost:7066/api/ledger/entries`
- **Get Trial Balance**: `GET https://localhost:7066/api/ledger/trial-balance`

## Troubleshooting

### Connection Refused
If you see `ERR_CONNECTION_REFUSED`, ensure that:
1. The application is actually running (`dotnet run`).
2. You are using the correct port (`7066` for HTTPS).
3. No other instances of the app are hung—check with `netstat -ano | findstr :7066`.