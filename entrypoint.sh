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

#export PATRONI_REPLICATION_USERNAME="$REPLICATION_NAME"
#export PATRONI_REPLICATION_PASSWORD="$REPLICATION_PASS"
#export PATRONI_SUPERUSER_USERNAME="$SU_NAME"
#export PATRONI_SUPERUSER_PASSWORD="$SU_PASS"

#export PATRONI_approle_PASSWORD="$POSTGRES_APP_ROLE_PASS"
#export PATRONI_approle_OPTIONS="${PATRONI_admin_OPTIONS:-createdb, createrole}"

chown -R postgres:postgres /var/lib/postgresql/patroni
chmod 700 /var/lib/postgresql/patroni


exec su -s /bin/sh postgres -c "/usr/bin/patroni /patroni.yml"
#exec /usr/bin/patroni /patroni.yml