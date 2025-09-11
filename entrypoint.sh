#!/bin/sh

readonly CONTAINER_IP=$(hostname --ip-address)

# адрес ip:port, где будет слушать patroni внутри контейнера 
readonly CONTAINER_API_ADDR="${CONTAINER_IP}:${PATRONI_API_CONNECT_PORT}" 
readonly CONTAINER_POSTGRE_ADDR="${CONTAINER_IP}:5432"

# Имя узла в кластере
export PATRONI_NAME="${PATRONI_NAME:-$(hostname)}"

# Адрес, по которому другие Patroni-узлы будут обращаться к этому узлу
export PATRONI_RESTAPI_CONNECT_ADDRESS="$CONTAINER_API_ADDR" 

# Где Patroni будет слушать входящие запросы
export PATRONI_RESTAPI_LISTEN="$CONTAINER_API_ADDR"

# Адрес, по которому другие узлы подключаются к PostgreSQL
export PATRONI_POSTGRESQL_CONNECT_ADDRESS="$CONTAINER_POSTGRE_ADDR"

# Внутренний порт, где слушает postgres
export PATRONI_POSTGRESQL_LISTEN="$CONTAINER_POSTGRE_ADDR"

export PATRONI_REPLICATION_USERNAME="$REPLICATION_NAME"
export PATRONI_REPLICATION_PASSWORD="$REPLICATION_PASS"
export PATRONI_SUPERUSER_USERNAME="$SU_NAME"
export PATRONI_SUPERUSER_PASSWORD="$SU_PASS"

export PATRONI_approle_PASSWORD="$POSTGRES_APP_ROLE_PASS"
export PATRONI_approle_OPTIONS="${PATRONI_admin_OPTIONS:-createdb, createrole}"

exec /usr/bin/patroni /patroni.yml