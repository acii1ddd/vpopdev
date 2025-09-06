#!/bin/bash

log() {
  echo "$(date '+%Y-%m-%d %H:%M:%S') [MASTER INIT] $1"
}

# если одна команда падает - падает скрипт
set -e

#postgresql.conf
#echo "wal_level=replica" >> ${PGDATA}/postgresql.conf
#echo "max_wal_senders=3" >> ${PGDATA}/postgresql.conf

#pg_hba.conf
echo "host replication replicator all md5" >> ${PGDATA}/pg_hba.conf
log "pg_hba.conf is configurated successfully."

#ожидаем готовности базы данных
until pg_isready -U postgres; do
  log "wait ..."
  sleep 2
done
log "Database is ready."

psql -U postgres -c "create role replicator with login replication password '123'"
log "Role replicator is created successfully"

psql -U postgres -c "alter role replicator connection limit 5;"
log "Connection limit is restricted to 5"