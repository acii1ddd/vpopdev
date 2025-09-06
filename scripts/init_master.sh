#!/bin/bash

#postgres.conf
echo "wal_level = replica" >> ${PGDATA}/postgresql.conf
echo "max_wal_senders = 3" >> ${PGDATA}/postgresql.conf

#pg_hba.conf
echo "host    replication     replicator      0.0.0.0/0               trust" >> ${PGDATA}/pg_hba.conf

#ожидаем готовности базы данных
until pg_isready -U maxim; do
  echo "wait ..."
  sleep 2
done

psql -U maxim -d elk -c "CREATE ROLE replicator WITH REPLICATION LOGIN;"