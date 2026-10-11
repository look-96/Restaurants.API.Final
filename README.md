# Restaurants API

A REST API for managing restaurants and their dishes. The project demonstrates a layered ASP.NET Core application with authentication, role-based access, data validation, persistence, and Azure Blob Storage integration for restaurant logos.

## Features

- Create, list, retrieve, update, and delete restaurants.
- Search, paginate, and sort the restaurant list.
- Create, list, retrieve, and delete dishes for a restaurant.
- Register and authenticate users with ASP.NET Core Identity API endpoints.
- Assign and remove user roles, and restrict selected restaurant operations to owners or administrators.
- Upload restaurant logos to Azure Blob Storage and return time-limited read URLs.
- Validate request data and handle errors through middleware.
- Seed initial restaurant data and roles, and apply pending EF Core migrations at startup.
- Explore the API with Swagger in the Development environment.

## Tech stack

- C# and ASP.NET Core 8
- Entity Framework Core and SQL Server
- ASP.NET Core Identity
- MediatR for command/query dispatch
- FluentValidation and AutoMapper
- Azure Blob Storage
- Serilog and Swagger / OpenAPI
- xUnit and MSTest-based test projects, with Moq and FluentAssertions

## Architecture

The solution is split into four projects:

| Project | Responsibility |
| --- | --- |
| `Restaurants.API` | HTTP controllers, startup configuration, middleware, and API documentation |
| `Restaurants.Application` | Use cases, commands, queries, DTOs, validation, and request mapping |
| `Restaurants.Domain` | Core entities, repository contracts, and domain interfaces/constants |
| `Restaurants.Infrastructure` | EF Core persistence, repositories, Identity, authorization, seeding, and blob storage |

Application requests are dispatched through MediatR handlers. Infrastructure implements the persistence and service interfaces used by the application layer.

## Prerequisites

- .NET 8 SDK
- SQL Server LocalDB (the default development connection string uses LocalDB), or another SQL Server instance
- Azurite or an Azure Storage account to use the logo upload feature (development configuration uses the Azurite connection string)

## Run locally

1. Clone the repository and change into its directory.
2. Set the `RestaurantsDb` connection string and `BlobStorage` settings for your environment. Development defaults are in `src/Restaurants.API/appsettings.Development.json`; use user secrets or environment variables for credentials instead of committing real secrets.
3. Restore and run the API:

   ```bash
   dotnet restore Restaurants.API.sln
   dotnet run --project src/Restaurants.API/Restaurants.API.csproj
   ```

At startup, the application applies pending EF Core migrations and seeds initial restaurants and roles. In Development, Swagger UI is enabled; open the URL printed by `dotnet run` and navigate to `/swagger`.

## API overview

The API uses the `/api` prefix. Endpoints marked with `*` require authentication or an appropriate role according to the controller configuration.

| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/api/restaurants` | List restaurants; supports search, paging, and sorting query parameters |
| `GET` | `/api/restaurants/{id}` * | Retrieve a restaurant |
| `POST` | `/api/restaurants` * | Create a restaurant (Owner role) |
| `PATCH` | `/api/restaurants/{id}` * | Update a restaurant |
| `DELETE` | `/api/restaurants/{id}` * | Delete a restaurant |
| `POST` | `/api/restaurants/{id}/logo` * | Upload a restaurant logo |
| `GET` | `/api/restaurants/{restaurantId}/dishes` | List a restaurant's dishes |
| `GET` | `/api/restaurants/{restaurantId}/dishes/{dishId}` | Retrieve a dish |
| `POST` | `/api/restaurants/{restaurantId}/dishes` | Create a dish |
| `DELETE` | `/api/restaurants/{restaurantId}/dishes` | Delete a restaurant's dishes |
| `POST` | `/api/identity/register` | Register a user (ASP.NET Core Identity endpoint) |
| `POST` | `/api/identity/login` | Authenticate a user (ASP.NET Core Identity endpoint) |
| `PATCH` | `/api/identity/user` * | Update the current user's details |
| `POST` / `DELETE` | `/api/identity/userRole` * | Assign or remove a role (Admin role) |

`GET /api/restaurants` accepts `searchPhrase`, `pageNumber`, `pageSize`, `sortBy`, and `sortDirection`. For example:

```http
GET /api/restaurants?searchPhrase=italian&pageNumber=1&pageSize=10&sortBy=Name&sortDirection=Ascending
```

## Run tests

Run the solution's automated tests with:

```bash
dotnet test Restaurants.API.sln
```

The test projects cover API, application, and infrastructure behavior.
