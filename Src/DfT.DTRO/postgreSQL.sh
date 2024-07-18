export PATH="/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin"
ls
psql --help
which psql
psql -h 127.0.0.1 -U 11jul -d dtro-dev-database -f D-TRO_Database.sql
exec DfT.DTRO
