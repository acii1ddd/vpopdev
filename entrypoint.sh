#!/bin/sh

set -e

log() {
  echo "$(date '+%Y-%m-%d %H:%M:%S') [MASTER INIT] $1"
}

# ports: 5432, 8008
readonly CONTAINER_IP=$(hostname --ip-address)

# адрес ip:port, где будет слушать patroni внутри контейнера 
readonly CONTAINER_API_ADDR="${CONTAINER_IP}:${PATRONI_API_CONNECT_PORT}" 
readonly CONTAINER_POSTGRE_ADDR="${CONTAINER_IP}:5432"

log "Container IP: ${CONTAINER_IP}"
log "CONTAINER_API_ADDR set to: ${CONTAINER_API_ADDR}"
log "CONTAINER_POSTGRE_ADDR set to: ${CONTAINER_POSTGRE_ADDR}"

# Имя узла в кластере
export PATRONI_NAME="${PATRONI_NAME:-$(hostname)}"

log "PATRONI_NAME set to: ${PATRONI_NAME}"

# Адрес, по которому другие Patroni-узлы будут обращаться к этому узлу
export PATRONI_RESTAPI_CONNECT_ADDRESS="$CONTAINER_API_ADDR" 

log "PATRONI_RESTAPI_CONNECT_ADDRESS set to: ${PATRONI_RESTAPI_CONNECT_ADDRESS}"

# Где Patroni будет слушать входящие запросы
export PATRONI_RESTAPI_LISTEN="$CONTAINER_API_ADDR"

log "PATRONI_RESTAPI_LISTEN set to: ${PATRONI_RESTAPI_LISTEN}"

# Адрес, по которому другие узлы подключаются к PostgreSQL
export PATRONI_POSTGRESQL_CONNECT_ADDRESS="$CONTAINER_POSTGRE_ADDR"

log "PATRONI_POSTGRESQL_CONNECT_ADDRESS set to: ${PATRONI_POSTGRESQL_CONNECT_ADDRESS}"

# Внутренний порт, где слушает postgres
export PATRONI_POSTGRESQL_LISTEN="$CONTAINER_POSTGRE_ADDR"

log "PATRONI_POSTGRESQL_LISTEN set to: ${PATRONI_POSTGRESQL_LISTEN}"

chown -R postgres:postgres /var/lib/postgresql/patroni/main
chmod 700 /var/lib/postgresql/patroni/main

echo ">>> Permissions fixed, starting Patroni"

exec su -s /bin/sh postgres -c "/usr/bin/patroni /patroni.yml"