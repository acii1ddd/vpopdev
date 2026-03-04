# ToDo Application

ToDo application with Feature-Sliced Architecture, including ASP.NET Core backend, high-availability PostgreSQL cluster, and load balancer.

## 📋 Overview

Task management application with a simple domain model:
- **User** — system user
- **ToDoItem** — task with fields: title, description, completion status, due dates

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                         Frontend                                │
│                    (Planned/In Development)                     │
│              React + TypeScript + FSD                           │
└─────────────────────────┬───────────────────────────────────────┘
                          │
                          │ HTTP/REST API
                          ▼
┌─────────────────────────────────────────────────────────────────┐
│                      Backend API                                │
│                  ASP.NET Core 10 + CQRS                         │
│                  Port 6767                                      │
└─────────────────────────┬───────────────────────────────────────┘
                          │
                          │ Connection Strings
                          │ (Read: 5433, Write: 5432)
                          ▼
┌─────────────────────────────────────────────────────────────────┐
│                         HAProxy                                 │
│              Port 5432 → Master (write)                         │
│              Port 5433 → Replicas (read, round-robin)           │
└─────────────────────────┬───────────────────────────────────────┘
                          │
          ┌───────────────┼───────────────┐
          │               │               │
    ┌─────▼─────┐   ┌─────▼─────┐   ┌─────▼─────┐
    │ Patroni-1 │   │ Patroni-2 │   │ Patroni-3 │
    │ PostgreSQL│   │ PostgreSQL│   │ PostgreSQL│
    └───────────┘   └───────────┘   └───────────┘
```

## 📁 Repository Structure

```
vpopdev/
├── ToDo.Frontend/         # Frontend Application (Planned)
│   ├── src/               # Source Code
│   ├── public/            # Static Assets
│   └── README.md          # Frontend Documentation
├── ToDo.Backend/          # Backend Application
│   ├── src/               # API Source Code
│   ├── tests/             # Unit & Integration Tests
│   └── README.md          # Backend Documentation
├── ToDo.Infra/            # Infrastructure
│   ├── compose.yml        # Docker Compose Configuration
│   ├── patroni.yml        # Patroni Configuration
│   ├── haproxy.cfg        # HAProxy Configuration
│   ├── prometheus.yml     # Monitoring Configuration
│   └── README.md          # Infrastructure Documentation
└── README.md              # This File
```

## 🛠 Tech Stack

### Frontend (Planned)
| Technology | Purpose |
|------------|---------|
| **React** | UI Framework |
| **TypeScript** | Type Safety |
| **Feature-Sliced Design (FSD)** | Architecture Pattern |

### Backend
| Technology | Purpose |
|------------|---------|
| **ASP.NET Core 10** | Web Framework |
| **Entity Framework Core 10** | ORM |
| **MediatR** | CQRS Pattern |
| **FluentValidation** | Data Validation |
| **JWT** | Authentication |
| **Scalar.AspNetCore** | OpenAPI Documentation |

### Infrastructure
| Component | Version | Purpose |
|-----------|---------|---------|
| **PostgreSQL** | 17.0 | Primary Database |
| **Patroni** | latest | PostgreSQL HA |
| **etcd** | 3.5.18 | DCS (Distributed Configuration Store) |
| **HAProxy** | 3.1.3 | Load Balancer |
| **Prometheus** | latest | Metrics Collection |
| **Grafana** | latest | Metrics Visualization |

## 🚀 Quick Start

### Requirements
- .NET 10 SDK
- Docker and Docker Compose

### Start Infrastructure

See detailed instructions in [Infrastructure README](./ToDo.Infra/README.md).

### Start Backend

See detailed instructions in [Backend README](./ToDo.Backend/README.md).
