#!/bin/bash

# Script Variables
ORG=$apigee_organisation
uuid=$(uuidgen)
TRA_ID=$SWA_CODE

echo "uuid: ${uuid}"
# Get OAuth access token
OAUTH_RESPONSE=$(curl -X POST "https://dtro-integration.dft.gov.uk/v1/oauth-generator" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -u ${CLIENT_ID}:${CLIENT_SECRET} \
  -d "grant_type=client_credentials")

# Extract access token and appId
access_token=$(echo "$OAUTH_RESPONSE" | jq -r '.access_token')
product=$(echo "$OAUTH_RESPONSE" | jq -r '.api_product_list')
echo " "
echo "Access token retrieved"
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
  RESPONSE=$(curl -X POST 'https://dtro-integration.dft.gov.uk/v1/dtroUsers/createFromBody' \
    -H 'X-Correlation-ID: 41ae0471-d7de-4737-907f-cab2f0089796' \
    -H 'Content-Type: application/json' \
    -H 'Accept: text/plain' \
    -H "Authorization: Bearer ${access_token}" \
    -d '{
       "id": "'"${uuid}"'",
       "xAppId": "'"${APP_ID}"'",
       "traId": '${TRA_ID}',
       "name": "'"${APP_NAME}"'",
       "prefix": "PUB",
       "userGroup": "tra"
     }')

  return_id=$(echo $RESPONSE | jq -r '.id')

  # Error checking and handling
  if [[ -n "$return_id" ]]; then
    echo " "
    echo "Publisher user ${APP_NAME}(TRA_ID:${TRA_ID}) registered to D-TRO"
    echo "${RESPONSE}"
  else
    echo " "
    echo "${RESPONSE}"
    exit 1
  fi
else
  # Add Consumer user to D-TRO
  RESPONSE=$(curl -X POST 'https://dtro-integration.dft.gov.uk/v1/dtroUsers/createFromBody' \
    -H 'X-Correlation-ID: 41ae0471-d7de-4737-907f-cab2f0089796' \
    -H 'Content-Type: application/json' \
    -H 'Accept: text/plain' \
    -H "Authorization: Bearer ${access_token}" \
    -d '{
       "id": "'"${uuid}"'",
       "xAppId": "'"${APP_ID}"'",
       "name": "'"${APP_NAME}"'",
       "prefix": "CON",
      "userGroup": "consumer"
    }')

  return_id=$(echo "$RESPONSE" | jq -r '.id')

  # Error checking and handling
  if [[ -n "$return_id" ]]; then
    echo " "
    echo "Consumer user ${APP_NAME} registered to D-TRO"
    echo "${RESPONSE}"
  else
    echo " "
    echo "${RESPONSE}"
    exit 1
  fi
fi
