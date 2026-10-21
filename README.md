# Tarsil .NET Internship

A collection of C# practice projects built during a .NET internship. The projects are grouped by day and cover console applications, asynchronous programming, and ASP.NET Core Web APIs.

## Prerequisites

- .NET 10 SDK
- .NET Entity Framework Core CLI (`dotnet-ef`) to apply the Day 5 database migrations

## Projects

| Day | Projects | Topics |
| --- | --- | --- |
| 1 | `HelloWorld`, `PersonalInfo`, `Calculator`, `FizzBuzz`, `GradeCalculator` | Console input/output, arithmetic, and basic control flow |
| 2 | `BankAccount`, `ListPractice`, `Methods`, `Shapes` | Classes, methods, collections, and object-oriented programming |
| 3 | `AsyncDemo`, `StudentManager` | Async/await, HTTP requests, and a JSON-backed student management console app |
| 4 | `TodoApi` | ASP.NET Core CRUD API using in-memory data |
| 5 | `TodoApi` | Async ASP.NET Core CRUD API using Entity Framework Core and SQLite |

Each project is independent; there is no solution file at the repository root.

## Run a project

Run commands from the repository root, substituting any project path from the table above:

```bash
dotnet run --project Day1/HelloWorld
```

For example, to run the student manager:

```bash
dotnet run --project Day3/StudentManager
```

## Todo APIs

Run either API with:

```bash
dotnet run --project Day4/TodoApi
```

or:

```bash
dotnet run --project Day5/TodoApi
```

Both APIs expose these routes under `/api/todos`:

| Method | Route | Purpose |
| --- | --- | --- |
| `GET` | `/api/todos` | List todos |
| `GET` | `/api/todos/{id}` | Get a todo |
| `GET` | `/api/todos/completed` | List completed todos |
| `POST` | `/api/todos` | Create a todo |
| `PUT` | `/api/todos/{id}` | Update a todo |
| `PATCH` | `/api/todos/{id}/complete` | Mark a todo complete |
| `DELETE` | `/api/todos/{id}` | Delete a todo |

The Day 5 API stores data in SQLite. Apply its checked-in migrations from the repository root before running it:

```bash
dotnet ef database update --project Day5/TodoApi
```

The connection string is in `Day5/TodoApi/appsettings.json`. In Development, both APIs expose an OpenAPI document at `/openapi/v1.json`.
