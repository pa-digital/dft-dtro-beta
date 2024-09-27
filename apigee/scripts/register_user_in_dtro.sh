#!/bin/bash

# Script Variables
ORG=$apigee_organisation
uuid=$(uuidgen)
TRA_ID=$SWA_CODE

# Get OAuth access token
OAUTH_RESPONSE=$(curl -X POST "https://dtro-integration.dft.gov.uk/v1/oauth-generator" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -u ${CLIENT_ID}:${CLIENT_SECRET} \
  -d "grant_type=client_credentials")

# Extract access token and appId
access_token=$(echo "$OAUTH_RESPONSE" | jq -r '.access_token')
echo " "
echo "Got access token"
echo " "

## Check Health of D-TRO Platform
#HEALTH_API_RESPONSE=$(curl -s -o /dev/null -w "%{http_code}" -X GET 'https://dtro-integration.dft.gov.uk/v1/healthApi' \
#  -H "Authorization: Bearer ${access_token}" \
#  -H "X-Correlation-ID: 41ae0471-d7de-4737-907f-cab2f0089796"
#)
#
## Error checking and handling
#if [ "$HEALTH_API_RESPONSE" -eq 200 ]; then
# echo "API is healthy"
#else
# echo "API is unhealthy. HTTP response code: $HEALTH_API_RESPONSE"
# exit 1
#fi

if [ "$IS_PUBLISHER" = true ]; then
  # Add Publisher user (tra) to D-TRO
  RESPONSE=$(curl -s -o /dev/null -w "%{http_code}" -X POST 'https://dtro-integration.dft.gov.uk/v1/dtroUsers/createFromBody' \
    -H 'X-Correlation-ID: 41ae0471-d7de-4737-907f-cab2f0089796' \
    -H 'Content-Type: application/json' \
    -H 'Accept: text/plain' \
    -H "Authorization: Bearer ${access_token}" \
    -d '{
       "id": "'"${uuid}"'",
       "xAppId": "'"${app_id}"'",
       "traId": '${TRA_ID}',
       "name": "'"${APP_NAME}"'",
       "prefix": "'"${APP_PREFIX}"'",
       "userGroup": "tra"
     }')

  echo "RESPONSE-tra"
  echo "${RESPONSE}"

#  response_json=$(echo "$RESPONSE" | jq )
#  echo "${response_json}"
  # Error checking and handling
  #  if [ "$RESPONSE" -eq 200 ]; then
  #    echo "${APP_NAME}(${app_id}) added to D-TR0"
  #  else
  #    echo "Failed to add ${APP_NAME}(${app_id}) to D-TR0. HTTP response code: $RESPONSE"
  #    exit 1
  #  fi
else
  # Add Consumer user to D-TRO
  APP_ID_CLEAN=$(echo "$APP_ID" | tr -d '"')
  RESPONSE=$(curl -i -X POST 'https://dtro-integration.dft.gov.uk/v1/dtroUsers/createFromBody' \
    -H 'X-Correlation-ID: 41ae0471-d7de-4737-907f-cab2f0089796' \
    -H 'Content-Type: application/json' \
    -H 'Accept: text/plain' \
    -H "Authorization: Bearer ${access_token}" \
    -d '{
      "id": "'${uuid}'",
      "xAppId": "'${APP_ID_CLEAN}'",
      "name": "'${APP_NAME}'",
      "prefix": "'${APP_PREFIX}'",
      "userGroup": "consumer"
    }')

  response_json=$(echo "$RESPONSE" | jq )
  echo "${response_json}"
  # Error checking and handling
  #  if [ "$RESPONSE" -eq 200 ]; then
  #    echo "${APP_NAME}(${app_id}) added to D-TR0"
  #  else
  #    echo "Failed to add ${APP_NAME}(${app_id}) to D-TR0. HTTP response code: $RESPONSE"
  #    exit 1
  #  fi
fi
