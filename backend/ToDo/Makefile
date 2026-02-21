.PHONY: migration-add migration-update migration-remove

# Создать новую миграцию
# Usage: make migration-add name=MigrationName
migration-add:
	dotnet ef migrations add $(name) -s ./ToDo.API -p ./ToDo.API/ --context ToDo.API.Data.AppDbContext --output-dir Data/Migrations

# Применить миграции к БД
migration-update:
	dotnet ef database update -s ./ToDo.API -p ./ToDo.API/ --context ToDo.API.Data.AppDbContext

# Удалить последнюю миграцию
migration-remove:
	dotnet ef migrations remove -s ./ToDo.API -p ./ToDo.API/ --context ToDo.API.Data.AppDbContext
