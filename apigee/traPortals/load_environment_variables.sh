#!/bin/bash

if [ -z "${env_name_prefix}" ]; then
  echo "Environment name prefix variable is not set."
  exit 1
fi

case "${env_name_prefix}" in
  "test")
    source test.sh
    ;;
  "int")
    source int.sh
    ;;
  "prod")
    source prod.sh
    ;;
  *)
    echo "Environment name prefix not found: ${env_name_prefix}."
    exit 1
    ;;
esac