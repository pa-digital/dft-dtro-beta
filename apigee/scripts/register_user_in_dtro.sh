#!/bin/bash

# Script Variables
ORG=$apigee_organisation
CLIENT_ID=$CLIENT_ID
CLIENT_SECRET=$CLIENT_SECRET
uuid=$(uuidgen)

OAUTH_RESPONSE=$(curl -s -X GET 'https://dtro-integration.dft.gov.uk/v1/oauth-generator' \
  -H 'Content-Type: application/x-www-form-urlencoded' \
  -u "${CLIENT_ID}:${CLIENT_SECRET}" \
  -d 'grant_type=client_credentials')

echo "OAUTH_RESPONSE"
echo "${OAUTH_RESPONSE}"

# Extract access token and appId
access_token=$(echo "$OAUTH_RESPONSE" | jq -r '.access_token')
app_id=$(echo "$OAUTH_RESPONSE" | jq -r '.application_name')

RESPONSE=$(curl -s -o /dev/null -w "%{http_code}" -X GET 'https://dtro-integration.dft.gov.uk/v1/healthApi' \
  -H "Authorization: Bearer ${access_token}" \
  -H "X-Correlation-ID: ${app_id}"
)

# Error checking and handling
 if [ "$RESPONSE" -eq 200 ]; then
   echo "API is healthy"
 else
   echo "API is unhealthy. HTTP response code: $RESPONSE"
 fi

#RESPONSE=$(curl -s -o /dev/null -w "%{http_code}" -X POST 'https://dtro-test.dft.gov.uk/v1/dtroUsers/createFromBody' \
#  -H 'X-Correlation-ID: 41ae0471-d7de-4737-907f-cab2f0089796' \
#  -H 'Content-Type: application/json' \
#  -H 'Accept: text/plain' \
#  -H "Authorization: Bearer ${TOKEN}" \
#  -d '{
#    "id": "'${uuid}'",
#    "traId": "<integer>",
#    "name": "<string>",
#    "prefix": "<string>",
#    "userGroup": "admin",
#    "xAppId": "<uuid>"
#  }')
#
## Error checking and handling
#  if [ "$RESPONSE" -eq 204 ]; then
#    echo "${APP_NAME} Activated."
#  else
#    echo "Failed to activate ${APP_NAME}. HTTP response code: $RESPONSE"
#  fi

