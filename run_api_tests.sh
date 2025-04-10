#!/bin/bash

# Default values
ENVIRONMENT="local"
ADMIN_CLIENT_ID=""
ADMIN_CLIENT_SECRET=""
CONSUMER_CLIENT_ID=""
CONSUMER_CLIENT_SECRET=""
PUBLISHER_CLIENT_ID_1=""
PUBLISHER_CLIENT_SECRET_1=""
PUBLISHER_TRA_ID_1=""

# Parse arguments
while [[ $# -gt 0 ]]; do
  case "$1" in
    -e) ENVIRONMENT="$2"; shift 2;;
    -ai) ADMIN_CLIENT_ID="$2"; shift 2;;
    -as) ADMIN_CLIENT_SECRET="$2"; shift 2;;
    -ci) CONSUMER_CLIENT_ID="$2"; shift 2;;
    -cs) CONSUMER_CLIENT_SECRET="$2"; shift 2;;
    -pi1) PUBLISHER_CLIENT_ID_1="$2"; shift 2;;
    -ps1) PUBLISHER_CLIENT_SECRET_1="$2"; shift 2;;
    -ti1) PUBLISHER_TRA_ID_1="$2"; shift 2;;
    -pi2) PUBLISHER_2_CLIENT_ID="$2"; shift 2;;
    -ps2) PUBLISHER_2_CLIENT_SECRET="$2"; shift 2;;
    -ti2) PUBLISHER_2_TRA_ID="$2"; shift 2;;
    --) shift; break;;
    *) echo "Unknown option: $1"; exit 1;;
  esac
done

echo "Environment: $ENVIRONMENT"

cd Src/DfT.DTRO.ApiTests
dotnet build
ENVIRONMENT="$ENVIRONMENT" ADMIN_CLIENT_ID="$ADMIN_CLIENT_ID" ADMIN_CLIENT_SECRET="$ADMIN_CLIENT_SECRET" CONSUMER_CLIENT_ID="$CONSUMER_CLIENT_ID" CONSUMER_CLIENT_SECRET="$CONSUMER_CLIENT_SECRET" PUBLISHER_CLIENT_ID_1="$PUBLISHER_CLIENT_ID_1" PUBLISHER_CLIENT_SECRET_1="$PUBLISHER_CLIENT_SECRET_1" PUBLISHER_TRA_ID_1="$PUBLISHER_TRA_ID_1" dotnet test --logger "html;LogFileName=test-results.html"
