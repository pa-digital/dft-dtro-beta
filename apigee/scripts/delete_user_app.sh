#!/bin/bash

# Script Variables
ORG=$apigee_organisation
TOKEN=$1

# Make the API call to approve api product
RESPONSE=$(curl -s -o /dev/null -w "%{http_code}" -X DELETE "https://apigee.googleapis.com/v1/organizations/${ORG}/developers/${DEV_EMAIL}/apps/${APP_NAME}" \
  -H "Authorization: Bearer ${TOKEN}")

# Error checking and handling
  if [ "$RESPONSE" -eq 200 ]; then
    echo "'${APP_NAME}' deleted."
  else
    echo "Failed to delete '${APP_NAME}'. HTTP response code: $RESPONSE"
    exit 1
  fi
