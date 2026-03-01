#!/bin/sh
set -e

echo ">>> post_bootstrap: creating ToDoDb database..."

# Подключаемся через unix-сокет (работает локально внутри контейнера)
psql -U postgres -h /var/run/postgresql -c "SELECT 1 FROM pg_database WHERE datname = 'ToDoDb';" | grep -q 1 || \
    psql -U postgres -h /var/run/postgresql -c "CREATE DATABASE \"ToDoDb\";"

echo ">>> post_bootstrap: database ToDoDb created successfully"