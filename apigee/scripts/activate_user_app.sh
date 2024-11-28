#!/bin/bash

# Script Variables
ORG=$apigee_organisation
TOKEN=$1

# Make the API call to retrieve developer's details
RESPONSE=$(curl -s -X GET "https://apigee.googleapis.com/v1/organizations/${ORG}/developers/${DEV_EMAIL}/apps/${APP_NAME}" \
  -H "Authorization: Bearer ${TOKEN}")

# Extract key and product name
consumerKey=$(echo "$RESPONSE" | jq -r '.credentials[0].consumerKey')
apiproduct=$(echo "$RESPONSE" | jq -r '.credentials[0].apiProducts[0].apiproduct')

# Make the API call to approve api product
RESPONSE=$(curl -s -o /dev/null -w "%{http_code}" -X POST "https://apigee.googleapis.com/v1/organizations/${ORG}/developers/${DEV_EMAIL}/apps/${APP_NAME}/keys/${consumerKey}/apiproducts/${apiproduct}?action=approve" \
  -H "Authorization: Bearer ${TOKEN}")
# Error checking and handling
  if [ "$RESPONSE" -eq 204 ]; then
    echo "${APP_NAME} Activated."
  else
    echo "Failed to activate ${APP_NAME}. HTTP response code: $RESPONSE"
    exit 1
  fi
