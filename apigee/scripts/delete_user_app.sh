#!/bin/bash

# Script Variables
ORG=$apigee_organisation
TOKEN=$1

# Make the API call to approve api product
RESPONSE=$(curl -i -X DELETE "https://apigee.googleapis.com/v1/organizations/${ORG}/developers/${DEV_EMAIL}/apps/${APP_NAME}" \
  -H "Authorization: Bearer ${TOKEN}")
# Error checking and handling
echo "$RESPONSE"
#  if [ "$RESPONSE" -eq 204 ]; then
#    echo "${APP_NAME} Activated."
#  else
#    echo "Failed to activate ${APP_NAME}. HTTP response code: $RESPONSE"
#  fi
