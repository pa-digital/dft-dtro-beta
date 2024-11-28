#!/bin/bash

# Script Variables
ID=$1

# Get OAuth access token
OAUTH_RESPONSE=$(curl -X POST "https://${DOMAIN}.dft.gov.uk/v1/oauth-generator" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -u ${CLIENT_ID}:${CLIENT_SECRET} \
  -d "grant_type=client_credentials")

# Extract access token and appId
if [ -z "$OAUTH_RESPONSE" ] || [ "$OAUTH_RESPONSE" == "[]" ]; then
  echo "Response is empty"
  exit 1
else
  access_token=$(echo "$OAUTH_RESPONSE" | jq -r '.access_token')
  echo " "
  echo "Access token retrieved"
  echo " "
fi

## Make API call
RESPONSE=$(curl -s -o /dev/null -w "%{http_code}" -X DELETE "https://${DOMAIN}.dft.gov.uk/v1/dtroUsers/redundant" \
  -H 'X-Correlation-ID: 41ae0471-d7de-4737-907f-cab2f0089796' \
  -H 'Content-Type: application/json' \
  -H 'Accept: text/plain' \
  -H "Authorization: Bearer ${access_token}" \
  -d '{
     "ids": ["'"${ID}"'"]
   }')

if [ "$RESPONSE" -eq 200 ]; then
    echo "User {${ID}} deleted."
  else
    echo "Failed to delete {${ID}}. HTTP response code: $RESPONSE"
    exit 1
fi
