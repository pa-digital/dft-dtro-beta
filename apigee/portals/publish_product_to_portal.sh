#!/bin/bash

# Script Variables
ORG=$apigee_organisation

to_title_case() {
  echo "$1" | sed -e 's/\b./\u&/g' -e 's/-/ /g'
}
TITLE_ENV=$(to_title_case "${env}")
PORTAL_NAME="${TITLE_ENV} Developer D-TRO Portal"
PORTAL_URL="$(echo "${PORTAL_NAME//[- ]/}" | tr '[:upper:]' '[:lower:]')"

TOKEN=$1

# List of product names
PRODUCT_NAMES=("central-service-provider" "digital-service-provider" "data-consumer")

# Loop through each product name
for PRODUCT in "${PRODUCT_NAMES[@]}"; do
  # Convert product name to title case with spaces
  TITLE=$(to_title_case "${PRODUCT}")

  # Construct the description
  DESCRIPTION="This is the ${TITLE_ENV} D-TRO application for ${PRODUCT}s."

  # Make the API call
  RESPONSE=$(curl -s -o /dev/null -w "%{http_code}" -X POST "https://apigee.googleapis.com/v1/organizations/${ORG}/sites/${ORG}-${PORTAL_URL}/apidocs" \
    -H "Authorization: Bearer ${TOKEN}" \
    -H "Content-Type: application/json" \
    -d '{
      "title": "'"${TITLE}"'",
      "description": "'"${DESCRIPTION}"'",
      "anonAllowed": true,
      "imageUrl": "",
      "requireCallbackUrl": false,
      "categoryIds": [],
      "published": true,
      "apiProductName": "'"${TITLE}"'"
    }')

 # Error checking and handling
  if [ "$RESPONSE" -eq 200 ]; then
    echo "${TITLE} successfully created in the Developer Portal."
  elif [ "$RESPONSE" -eq 409 ]; then
    echo "${TITLE} already exists in the Developer Portal."
  else
    echo "Failed to publish ${TITLE} to developer portal ${PORTAL_NAME}. HTTP response code: $RESPONSE"
    exit 1
  fi
done

RESPONSE=$(curl -s -o /dev/null -X POST "https://apigee.googleapis.com/v1/organizations/${ORG}/sites/${ORG}-${PORTAL_URL}/apidocs" \
    -H "Authorization: Bearer ${TOKEN}" \
    -H "Content-Type: application/json" \
    -d '{
      "title": "test1",
      "description": "test1",
      "anonAllowed": true,
      "imageUrl": "",
      "requireCallbackUrl": false,
      "categoryIds": [],
      "published": true,
      "apiProductName": "test1"
    }')

 echo "${RESPONSE}"

