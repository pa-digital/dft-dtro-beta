#!/bin/bash

# Get OAuth access token
OAUTH_RESPONSE=$(curl -X POST "https://dtro-integration.dft.gov.uk/v1/oauth-generator" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -u ${CLIENT_ID}:${CLIENT_SECRET} \
  -d "grant_type=client_credentials")

# Extract access token and appId
access_token=$(echo "$OAUTH_RESPONSE" | jq -r '.access_token')
echo " "
echo "Access token retrieved"
echo " "

## Check Health of D-TRO Platform
RESPONSE=$(curl -X GET 'https://dtro-integration.dft.gov.uk/v1/dtroUsers' \
  -H "Authorization: Bearer ${access_token}" \
  -H "X-Correlation-ID: 41ae0471-d7de-4737-907f-cab2f0089796" \
  -H 'Accept: text/plain'
)
echo " "
echo "Response for Retrieve All Dtro Users:"
echo " "
echo "$RESPONSE"
