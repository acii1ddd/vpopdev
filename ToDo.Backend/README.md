# ToDo.Backend

Backend API для приложения ToDo, написанное на **ASP.NET Core 10**.

## 📋 Описание

REST API для управления задачами (ToDo) с поддержкой:
- CRUD операции для задач
- JWT аутентификация
- Версионирование API
- PostgreSQL (раздельные подключения для чтения/записи)
- OpenAPI/Swagger документация через Scalar

## 🛠 Технологии

- **.NET 10**
- **ASP.NET Core** (Minimal APIs)
- **Entity Framework Core 10**
- **PostgreSQL**
- **MediatR** — CQRS паттерн
- **FluentValidation** — валидация
- **JWT** — аутентификация
- **Scalar.AspNetCore** — OpenAPI UI

## 📁 Структура проекта

```
src/
├── ToDo.API/              # Основной API проект
│   ├── ConfigurationExtensions/  # DI расширения
│   ├── Data/              # EF Core, DbContext, репозитории
│   ├── Dtos/              # DTO классы
│   ├── EndpointSettings/  # Настройки эндпоинтов
│   ├── Features/          # Фичи по CQRS (ToDos, Users)
│   └── Program.cs
├── ToDo.Shared/           # Общие классы и утилиты
tests/
├── ToDo.API.Tests/        # Unit/API тесты
└── ToDo.Features.Tests/   # Тесты фич
```

## 🚀 Быстрый старт

### Требования

- .NET 10 SDK
- PostgreSQL (два инстанса: порт 5432 — запись, 5433 — чтение)

### Запуск

```bash
dotnet restore
dotnet run --project src/ToDo.API
```

API будет доступно на `http://localhost:6767`

### Миграции

```bash
# Создать миграцию
make migration-add name=MigrationName

# Применить миграции
make migration-update

# Откатить последнюю миграцию
make migration-remove
```

## ⚙️ Конфигурация

Основные настройки в `appsettings.json`:

| Параметр | Описание |
|----------|----------|
| `ConnectionStrings:ReadConnection` | PostgreSQL для чтения (порт 5433) |
| `ConnectionStrings:WriteConnection` | PostgreSQL для записи (порт 5432) |
| `Jwt:Key` | Секретный ключ для JWT |
| `Jwt:Issuer` | Издатель токена |
| `Jwt:Audience` | Аудитория токена |
| `Jwt:ExpiresInMinutes` | Время жизни токена (мин) |

## 📦 API Endpoints

| Метод | Endpoint | Описание |
|-------|----------|----------|
| GET | `/api/v1/todos` | Получить все задачи |
| GET | `/api/v1/todos/{id}` | Получить задачу по ID |
| POST | `/api/v1/todos` | Создать задачу |
| PUT | `/api/v1/todos/{id}` | Обновить задачу |
| DELETE | `/api/v1/todos/{id}` | Удалить задачу |

## 🧪 Тесты

```bash
dotnet test
```

## 📝 Статус разработки

Проект в активной разработке. Возможны изменения в API и структуре.
