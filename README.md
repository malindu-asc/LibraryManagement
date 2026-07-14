# Library Management API

A .NET Minimal Web API for managing library books, members, and borrowings, built with EF Core and PostgreSQL.

## Technologies Used

- .NET 10 Minimal APIs
- Entity Framework Core (Npgsql provider)
- PostgreSQL (via Docker Compose)
- Swagger / OpenAPI 
- Repository pattern with an application service layer

## Project Structure

```
Library.Api/
  Endpoints/       Minimal API endpoint definitions and the validation filter
  Contracts/        Request/response DTOs, grouped by resource
  Domain/            Entities, enums, and domain exceptions
  Application/      Repository/service interfaces and service implementations
  Infrastructure/   EF Core DbContext, repositories, and data seeding
  Middleware/         Global exception handling
  Migrations/          EF Core migrations
```

## Prerequisites

- .NET 10 SDK
- Docker (for running PostgreSQL)

## Running the Database

Start PostgreSQL with Docker Compose from the repository root:

```bash
docker-compose up -d
```

This starts a `library-postgres` container with database `librarydb`, user `libraryuser`, and password `librarypassword`, exposed on `localhost:5432`.

## Applying Migrations

Migrations are applied automatically on startup 
```bash
cd Library.Api
dotnet tool restore
dotnet ef database update
```

## Running the API

```bash
cd Library.Api
dotnet run
```

On startup the API will:
1. Apply any pending EF Core migrations.
2. Seed sample data (5 books, 3 members, 2 active borrowings) if the database is empty.

The API listens on `http://localhost:5120` by default. Swagger UI is available at `http://localhost:5120/swagger`.

## Seed Data

`Infrastructure/Data/DataSeeder.cs` seeds sample data the first time the app runs against an empty database:
- 5 books, including one (*Refactoring*) seeded with only 1 total copy that's immediately borrowed, leaving it with 0 available copies to exercise availability rules
- 3 members (two of which start with active borrowings)
- 2 borrowings, so `GET /api/borrowings` and `GET /api/members/{id}/borrowings` return data immediately

No manual steps are needed — seeding happens automatically after migrations run.

## Example API Requests

Create a book:

```bash
curl -X POST http://localhost:5120/api/books \
  -H "Content-Type: application/json" \
  -d '{"title":"The Hobbit","author":"J.R.R. Tolkien","isbn":"9780618260300","publishedYear":1937,"totalCopies":2}'
```

Register a member:

```bash
curl -X POST http://localhost:5120/api/members \
  -H "Content-Type: application/json" \
  -d '{"fullName":"Dana Lee","email":"dana.lee@example.com","phoneNumber":"0712345678"}'
```

Borrow a book:

```bash
curl -X POST http://localhost:5120/api/borrowings \
  -H "Content-Type: application/json" \
  -d '{"bookId":"<book-id>","memberId":"<member-id>"}'
```

Return a book:

```bash
curl -X POST http://localhost:5120/api/borrowings/<borrowing-id>/return
```

View a member's borrowing history:

```bash
curl http://localhost:5120/api/members/<member-id>/borrowings
```

## Business Rules

- A book can only be borrowed while `AvailableCopies > 0`.
- Only active members can borrow.
- A member cannot hold more than 3 active borrowings at once.
- Borrowing/returning adjusts `AvailableCopies` by one; a borrowing cannot be returned twice.
- Due dates are always 14 days from the borrow date.
- ISBNs and member emails must be unique.

## Assumptions Made During Implementation

- `PublishedYear` is validated against the current UTC year; a year equal to the current year is allowed, anything later is rejected.
- `Overdue` is a defined status on `Borrowing`, but nothing currently transitions a borrowing from `Borrowed` to `Overdue` automatically — this would need a background job or a computed check against `DueDate`, which is out of scope for this submission.
- The connection string and PostgreSQL credentials are checked into `appsettings.json` for convenience since this is a local/assessment setup, not a production deployment.
