#!/bin/bash
set -e

log() {
  echo "$(date '+%Y-%m-%d %H:%M:%S') [SLAVE INIT] $1"
}

PGPASS_PATH="/var/lib/postgresql/.pgpass"
export PGPASSFILE="$PGPASS_PATH" # другие могут использовать в рамках скрипта (pg_basepackup)

if [ ! -s "$PGDATA/PG_VERSION" ]; then
    log "Creation .pgpass for connecting to a master..."
    #su - postgres -c "echo 'master:5432:*:replicator:123' > $PGPASS_PATH"
    #su - postgres -c "chmod 0600 $PGPASS_PATH"
    echo "master:5432:*:replicator:123" > "$PGPASS_PATH"
    chown postgres:postgres /var/lib/postgresql/.pgpass
    chmod 0600 "$PGPASS_PATH"

    log ".pgpass file is configurated successfully."

    # log "Data folder is cleaning..."
    # rm -rf /var/lib/postgresql/data/*
    # log "Data folder is cleaned successfully."

    log "Backup start up..."
    pg_basebackup -h master -p 5432 -U replicator -D /var/lib/postgresql/data -Fp -Xs -P -R
    log "Backup is done successfully."
else
    log "Data directory already exists, skipping initialization"
fi
    exec /usr/local/bin/docker-entrypoint.sh postgres