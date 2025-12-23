# General Ledger Service Implementation Plan

Build a .NET 8 microservice for double-entry accounting with PostgreSQL.

## Proposed Changes

### [GeneralLedgerService]

#### [NEW] [Project Structure]
- Initialize .NET 8 Web API.
- Folders: `Domain`, `Models`, `Data`, `Controllers`, `Services`.

#### [NEW] [Models]
- `Account.cs`: `Id`, `Name`, `Code`, `Type` (Asset, Liability, Equity, Revenue, Expense).
- `JournalEntry.cs`: `Id`, `Date`, `Description`, `Reference`, `Lines`.
- `JournalEntryLine.cs`: `Id`, `AccountId`, `Amount` (Debit/Credit), `Description`.

#### [NEW] [Data]
- `LedgerDbContext.cs`: EF Core context for PostgreSQL.
- Configuration for `decimal` precision and relationships.

#### [NEW] [Services]
- `LedgerService.cs`: Business logic for posting entries, ensuring Debits == Credits.

## Verification Plan

### Automated Tests
- Unit tests for `JournalEntry` validation (ensure balanced entries).
- Integration tests for posting entries to the database.

### Manual Verification
- Use Swagger to create accounts and post journal entries.
- Verify Trial Balance report via API.
