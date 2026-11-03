# LibraryApi

A RESTful ASP.NET Core 10 Web API for managing a library's authors and books, backed by PostgreSQL. Migrations are applied automatically on startup via a custom SQL-file runner — no `dotnet ef` required.

---

## Table of Contents

- [Prerequisites](#prerequisites)
- [Database Setup](#database-setup)
- [User Secrets](#user-secrets)
- [Running the API](#running-the-api)
- [Auto-Migration & Seeding](#auto-migration--seeding)
- [Endpoints](#endpoints)
- [Error Responses](#error-responses)

---

## Prerequisites

| Tool | Minimum Version | Notes |
|------|----------------|-------|
| [.NET SDK](https://dotnet.microsoft.com/download) | 10.0 | `dotnet --version` to verify |
| [PostgreSQL](https://www.postgresql.org/download/) | 14+ | Must be running locally |
| [Postman](https://www.postman.com/downloads/) _(optional)_ | any | Import `LibraryApi.postman_collection.json` |

---

## Database Setup

Open a `psql` shell as a superuser (e.g., `postgres`) and run the following commands **once**:

### 1. Create a dedicated database user

```sql
CREATE USER library_user WITH PASSWORD 'your_strong_password';
```

### 2. Create the database

```sql
CREATE DATABASE library_db OWNER library_user;
```

### 3. Grant privileges

```sql
GRANT ALL PRIVILEGES ON DATABASE library_db TO library_user;
```

### 4. Connect to the new database and grant schema privileges

```sql
\c library_db
GRANT ALL ON SCHEMA public TO library_user;
```

> **Note:** You do **not** need to create any tables manually. The app runs all SQL files in `db/Migrations/` on startup and seeds initial data from `db/seed.sql` automatically.

---

## User Secrets

The connection string is stored in [.NET User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) so it is never committed to source control.

### Initialize (only needed once, already done if `UserSecretsId` is in the `.csproj`)

```bash
dotnet user-secrets init
```

### Set the connection string

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" \
  "Host=localhost;Port=5432;Database=library_db;Username=library_user;Password=your_strong_password"
```

Replace `your_strong_password` with the password you chose in the database setup step.

### Verify the secret was saved

```bash
dotnet user-secrets list
```

Expected output:
```
ConnectionStrings:DefaultConnection = Host=localhost;Port=5432;Database=library_db;Username=library_user;Password=your_strong_password
```

---

## Running the API

```bash
dotnet run
```

The API starts on:

| Scheme | URL |
|--------|-----|
| HTTP   | `http://localhost:5194` |
| HTTPS  | `https://localhost:7192` |

### Interactive API Docs (Development only)

| Tool | URL |
|------|-----|
| OpenAPI JSON | `http://localhost:5194/openapi/v1.json` |
| Scalar UI    | `http://localhost:5194/scalar/v1` |

---

## Auto-Migration & Seeding

On every startup the `DatabaseMigrator` service:

1. Creates the `schema_migrations` tracking table if it does not exist.
2. Reads every `*.sql` file inside `db/Migrations/` in **sorted filename order**.
3. Skips any migration whose filename is already recorded in `schema_migrations`.
4. Executes each unapplied migration inside a **transaction** and records it on success.
5. Runs `db/seed.sql` with `ON CONFLICT DO NOTHING` — safe to run repeatedly.

Current migrations:

| File | Description |
|------|-------------|
| `001_create_tables.sql` | Creates `authors`, `books`, `members`, and `loans` tables |
| `002_add_books_title_index.sql` | Adds `idx_books_title` index on `books.title` |

Seed data includes 3 authors (Dostoevsky, Kafka, Camus), 8 books, 5 members, and 7 loans.

---

## Endpoints

Base URL: `http://localhost:5194`

### Authors — `/api/authors`

| Method | Path | Description | Success |
|--------|------|-------------|---------|
| `GET` | `/api/authors` | List all authors | `200 OK` |
| `GET` | `/api/authors/{id}` | Get a single author by ID | `200 OK` |
| `POST` | `/api/authors` | Create a new author | `201 Created` |
| `PUT` | `/api/authors/{id}` | Replace an author's details | `200 OK` |
| `DELETE` | `/api/authors/{id}` | Delete an author | `204 No Content` |

#### `POST / PUT` request body — Author

```json
{
  "name": "George Orwell",
  "country": "United Kingdom"
}
```

#### Author response shape

```json
{
  "id": 4,
  "name": "George Orwell",
  "country": "United Kingdom"
}
```

---

### Books — `/api/books`

| Method | Path | Description | Success |
|--------|------|-------------|---------|
| `GET` | `/api/books` | List books (filterable, paginated) | `200 OK` |
| `GET` | `/api/books/{id}` | Get a single book by ID | `200 OK` |
| `POST` | `/api/books` | Create a new book | `201 Created` |
| `PUT` | `/api/books/{id}` | Replace a book's details | `204 No Content` |
| `DELETE` | `/api/books/{id}` | Delete a book | `204 No Content` |

#### `GET /api/books` — Query parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `page` | `int` | `1` | Page number (1-based) |
| `pageSize` | `int` | `10` | Items per page |
| `title` | `string` | — | Case-insensitive partial match on title |
| `authorId` | `int` | — | Filter by author |
| `minPrice` | `decimal` | — | Minimum price (inclusive) |
| `maxPrice` | `decimal` | — | Maximum price (inclusive) |

Example:
```
GET /api/books?title=crime&authorId=1&page=1&pageSize=5
```

#### `POST / PUT` request body — Book

```json
{
  "title": "1984",
  "isbn": "9780451524935",
  "price": 14.99,
  "totalCopies": 10,
  "availableCopies": 10,
  "publishedYear": 1949,
  "authorId": 1
}
```

#### Book response shape

```json
{
  "id": 9,
  "title": "1984",
  "isbn": "9780451524935",
  "price": 14.99,
  "totalCopies": 10,
  "availableCopies": 10,
  "publishedYear": 1949,
  "authorId": 1
}
```

---

## Error Responses

| Status | Trigger |
|--------|---------|
| `400 Bad Request` | `id <= 0` in path, `availableCopies > totalCopies`, `price <= 0`, or duplicate author name |
| `404 Not Found` | Resource with the given ID does not exist |
| `409 Conflict` | Deleting an author who still has books (foreign key restriction) |
| `500 Internal Server Error` | Unexpected server error — see server logs for details |

---

## Postman Collection

Import `LibraryApi.postman_collection.json` from the project root.

Set `{{baseUrl}}` to `http://localhost:5194` (already the default).

**Run the folders in order** so chained variables resolve correctly:

```
1. Authors          → POST "Create Author" captures {{authorId}}
2. Books            → POST "Create Book" uses {{authorId}}, captures {{bookId}}
3. Error Scenarios  → standalone 400 / 404 / 409 edge-case tests
4. Cleanup          → DELETE Book then DELETE Author (FK order matters)
```

