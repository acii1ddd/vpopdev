# ToDo.Backend

Backend API for the ToDo application built with **ASP.NET Core 10**.

## 📋 Overview

REST API for task management (ToDo) with support for:
- CRUD operations for tasks
- JWT authentication
- API versioning
- PostgreSQL (separate read/write connections)
- OpenAPI/Swagger documentation via Scalar

## 🛠 Tech Stack

- **.NET 10**
- **ASP.NET Core** (Minimal APIs)
- **Entity Framework Core 10**
- **PostgreSQL**
- **MediatR** — CQRS pattern
- **FluentValidation** — validation
- **JWT** — authentication
- **Scalar.AspNetCore** — OpenAPI UI

## 📁 Project Structure

```
src/
├── ToDo.API/              # Main API Project
│   ├── ConfigurationExtensions/  # DI extensions
│   ├── Data/              # EF Core, DbContext, repositories
│   ├── Dtos/              # DTO classes
│   ├── EndpointSettings/  # Endpoint settings
│   ├── Features/          # CQRS features (ToDos, Users)
│   └── Program.cs
├── ToDo.Shared/           # Shared classes and utilities
tests/
├── ToDo.API.Tests/        # Unit/API Tests
└── ToDo.Features.Tests/   # Feature tests
```

## 🚀 Quick Start

### Requirements

- .NET 10 SDK
- PostgreSQL (two instances: port 5432 — write, port 5433 — read)

### Run

```bash
dotnet restore
dotnet run --project src/ToDo.API
```

API will be available at `http://localhost:6767`

### Migrations

```bash
# Add migration
make migration-add name=MigrationName

# Apply migrations
make migration-update

# Rollback last migration
make migration-remove
```

## ⚙️ Configuration

Main settings in `appsettings.json`:

| Parameter | Description |
|-----------|-------------|
| `ConnectionStrings:ReadConnection` | PostgreSQL for read (port 5433) |
| `ConnectionStrings:WriteConnection` | PostgreSQL for write (port 5432) |
| `Jwt:Key` | Secret key for JWT |
| `Jwt:Issuer` | Token issuer |
| `Jwt:Audience` | Token audience |
| `Jwt:ExpiresInMinutes` | Token lifetime (minutes) |

## 📦 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/todos` | Get all tasks |
| GET | `/api/v1/todos/{id}` | Get task by ID |
| POST | `/api/v1/todos` | Create task |
| PUT | `/api/v1/todos/{id}` | Update task |
| DELETE | `/api/v1/todos/{id}` | Delete task |

## 🧪 Tests

```bash
dotnet test
```

## 📝 Development Status

Project is under active development. API and structure changes are possible.

## 📈 Roadmap

Planned backend enhancements:

- [ ] **User CRUD** — user management via API
- [ ] **Dockerize an application**
- [ ] **Serilog + ELK Stack** — centralized logging (Elasticsearch, Logstash, Kibana)
- [ ] **Redis Cache** — caching frequently requested data
- [ ] **Load Testing** — determine maximum system RPS and optimize if needed
