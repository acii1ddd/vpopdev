#!/bin/bash
#проверяет есть ли postgres если нет то начинает backup 
if [ ! -s "$PGDATA/PG_VERSION" ]; then
  echo "wait for data from master ..."
  pg_basebackup -h master -U replicator -D "$PGDATA" -Fp -Xs -R -P
else
  echo "skip init"
fi

exec docker-entrypoint.sh postgres
