#!/bin/bash

FILE=$1

if [ -z "$FILE" ]; then
    echo -e "\033[1;31mError: No file path provided.\033[0m"
    echo "Usage: $0 /path/to/file"
    exit 1
fi

if [ ! -f "$FILE" ]; then
    echo -e "\033[1;31mError: The file '$FILE' does not exist or is not a regular file.\033[0m"
    exit 1
fi

source .env
DB_NAME=DTRO
TABLE_NAME=DtroUsers
CONTAINER_NAME=postgres
CONTAINER_ID=$(docker ps --filter "name=$CONTAINER_NAME" -q)

if [ -n "$CONTAINER_ID" ]; then
    cat "$FILE" | docker exec -i $CONTAINER_ID psql -U $POSTGRES_USER -d $POSTGRES_DB -c "\copy \"$TABLE_NAME\" FROM STDIN DELIMITER ',' CSV HEADER;"
else
    echo -e "\033[1;31mNo container found matching the filter '$CONTAINER_NAME'\033[0m"
    exit 1
fi
