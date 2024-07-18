#!/bin/sh

apk --update add postgresql-client
chmod +x ./D-TRO_Database.sql
psql -h 127.0.0.1 -U 11jul -d dtro-dev-database -f ./D-TRO_Database.sql

exec ./Dft.DTRO