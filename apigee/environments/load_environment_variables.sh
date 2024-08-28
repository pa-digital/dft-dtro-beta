#!/bin/bash

if [ -z "${env_name_prefix}" ]; then
  echo "Environment variable is not set."
  exit 1
fi

case "${env_name_prefix}" in
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
    echo "Environment not found: ${env_name_prefix}."
    exit 1
    ;;
esac