#!/bin/bash

if [ -z "${environment}" ]; then
  echo "Environment variable is not set."
  exit 1
fi

case "${environment}" in
  "dev")
    source environments/dev.sh
    ;;
  "test")
    source environments/test.sh
    ;;
  "int")
    source environments/int.sh
    ;;
  "prod")
    source environments/prod.sh
    ;;
  *)
    echo "Environment not found: ${environment}."
    exit 1
    ;;
esac