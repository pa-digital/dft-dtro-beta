#!/bin/bash

#Add PostgreSQL 'D-TRO_Database.sql' script
apk add postgresql-client
chmod +x /app/D-TRO_Database.sql
psql -h 127.0.0.1 -U 11jul -d dtro-dev-database -f /app/D-TRO_Database.sql

exec ./Dft.DTRO