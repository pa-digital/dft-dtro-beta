#!/bin/bash

# Default values
ENVIRONMENT="local"
ADMIN_CLIENT_ID=""
ADMIN_CLIENT_SECRET=""
CONSUMER_CLIENT_ID=""
CONSUMER_CLIENT_SECRET=""
PUBLISHER_1_CLIENT_ID=""
PUBLISHER_1_CLIENT_SECRET=""
PUBLISHER_1_TRA_ID=""

# Parse arguments
while [[ $# -gt 0 ]]; do
  case "$1" in
    -e) ENVIRONMENT="$2"; shift 2;;
    -ai) ADMIN_CLIENT_ID="$2"; shift 2;;
    -as) ADMIN_CLIENT_SECRET="$2"; shift 2;;
    -ci) CONSUMER_CLIENT_ID="$2"; shift 2;;
    -cs) CONSUMER_CLIENT_SECRET="$2"; shift 2;;
    -p1i) PUBLISHER_1_CLIENT_ID="$2"; shift 2;;
    -p1s) PUBLISHER_1_CLIENT_SECRET="$2"; shift 2;;
    -t1i) PUBLISHER_1_TRA_ID="$2"; shift 2;;
    -p2i) PUBLISHER_1_CLIENT_ID="$2"; shift 2;;
    -p2s) PUBLISHER_1_CLIENT_SECRET="$2"; shift 2;;
    -t2i) PUBLISHER_1_TRA_ID="$2"; shift 2;;
    --) shift; break;;
    *) echo "Unknown option: $1"; exit 1;;
  esac
done

echo "Environment: $ENVIRONMENT"

cd Src/DfT.DTRO.IntegrationTests
dotnet build
ENVIRONMENT="$ENVIRONMENT" ADMIN_CLIENT_ID="$ADMIN_CLIENT_ID" ADMIN_CLIENT_SECRET="$ADMIN_CLIENT_SECRET" CONSUMER_CLIENT_ID="$CONSUMER_CLIENT_ID" CONSUMER_CLIENT_SECRET="$CONSUMER_CLIENT_SECRET" PUBLISHER_1_CLIENT_ID="$PUBLISHER_1_CLIENT_ID" PUBLISHER_1_CLIENT_SECRET="$PUBLISHER_1_CLIENT_SECRET" PUBLISHER_1_TRA_ID="$PUBLISHER_1_TRA_ID" dotnet test --logger "html;LogFileName=test-results.html"
