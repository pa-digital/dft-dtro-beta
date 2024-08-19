#!/bin/bash

# Script Variables
ORG=$apigee_organisation
TOKEN=$1

  # Make the API call
  RESPONSE=$(curl -s -o /dev/null -w "%{http_code}" -X POST "https://apigee.googleapis.com/v1/organizations/${ORG}/sites" \
    -H "Authorization: Bearer ${TOKEN}" \
    -H "Content-Type: application/json" \
    -d '{
      "name": "'"${tra^} D-TRO Portal"'",
      "description": "'"This is the ${tra^} Portal for D-TRO"'"
    }')

  # Error checking and handling
  if [ "$RESPONSE" -eq 200 ]; then
    echo "${tra^} Portal successfully created in ${ORG}."
  elif [ "$RESPONSE" -eq 409 ]; then
    echo "${tra^} Portal already exists in ${ORG}."
  else
    echo "Failed to create ${tra^} portal in ${ORG}. HTTP response code: $RESPONSE"
  fi
