# Inventory API

An ASP.NET Core Web API for managing inventory products and categories. It uses Entity Framework Core with SQLite for data storage.

## Requirements

- .NET 10 SDK

## Run

From the repository root:

```bash
dotnet run --project Day6/InventoryApi
```

The HTTP launch profile listens at `http://localhost:5137`. The HTTPS profile listens at `https://localhost:7199` and `http://localhost:5137`.

The SQLite connection string is configured in `appsettings.json`. In Development, the OpenAPI document is available at `/openapi/v1.json`.

## Endpoints

### Products

| Method | Route | Purpose |
| --- | --- | --- |
| `GET` | `/api/products` | List products. Optional query parameters: `categoryId`, `search`, `minPrice`, and `maxPrice`. |
| `GET` | `/api/products/{id}` | Get a product by ID. |
| `POST` | `/api/products` | Create a product. |
| `PUT` | `/api/products/{id}` | Update a product. |
| `DELETE` | `/api/products/{id}` | Delete a product. |
| `GET` | `/api/products/low-stock?threshold={threshold}` | List products whose stock is at or below the specified threshold. |
| `POST` | `/api/products/{id}/stock` | Increase a product's stock. |

### Categories

| Method | Route | Purpose |
| --- | --- | --- |
| `GET` | `/api/categories` | List categories. |
| `GET` | `/api/categories/{id}` | Get a category by ID. |
| `POST` | `/api/categories` | Create a category. |
| `PUT` | `/api/categories/{id}` | Update a category. |
| `DELETE` | `/api/categories/{id}` | Delete a category. |
