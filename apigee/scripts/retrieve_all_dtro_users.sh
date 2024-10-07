#!/bin/bash

DOMAIN=$domain

# Get OAuth access token
OAUTH_RESPONSE=$(curl -X POST "https://${DOMAIN}.dft.gov.uk/v1/oauth-generator" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -u ${CLIENT_ID}:${CLIENT_SECRET} \
  -d "grant_type=client_credentials")

# Extract access token and appId
access_token=$(echo "$OAUTH_RESPONSE" | jq -r '.access_token')
echo " "
echo "Access token retrieved"
echo " "

## Get
RESPONSE=$(curl -X GET "https://${DOMAIN}.dft.gov.uk/v1/dtroUsers" \
  -H 'X-Correlation-ID: 41ae0471-d7de-4737-907f-cab2f0089796' \
  -H 'Accept: text/plain' \
  -H "Authorization: Bearer ${access_token}"
)
number_of_users=$(echo "$RESPONSE" | jq '. | length')
echo " "
echo "Number of users found: $number_of_users"
echo " "
echo "$RESPONSE" | jq .
