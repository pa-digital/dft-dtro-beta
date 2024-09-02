#!/bin/bash

# Script Variables
ORG=$apigee_organisation
YAML_FILE="../openApi/openapi3_0.yml"
echo "files in path"
for file in ../openApi/*; do
    if [ -f "$file" ]; then
        echo "$file"
    fi
done
to_title_case() {
  echo "$1" | sed -e 's/\b./\u&/g' -e 's/-/ /g'
}
TITLE=$(to_title_case "${tra}")
PORTAL_NAME="${TITLE} D-TRO Portal"
PORTAL_URL="$(echo "${PORTAL_NAME//[- ]/}" | tr '[:upper:]' '[:lower:]')"

TOKEN=$1

# List of product names
PRODUCT_NAMES=("digital-service-provider" "data-consumer")

# Loop through each product name
for PRODUCT in "${PRODUCT_NAMES[@]}"; do

  # Name of product created via product.json
  PRODUCT_NAME="${env_name_prefix}-${PRODUCT}"

  # Convert product name to title case with spaces
  TITLE=$(to_title_case "${PRODUCT}")

  # Construct the description
  DESCRIPTION="This is the D-TRO application for ${PRODUCT}s."

  # Make the API call
  RESPONSE=$(curl -s -o /dev/null -w "%{http_code}" -X POST "https://apigee.googleapis.com/v1/organizations/${ORG}/sites/${ORG}-${PORTAL_URL}/apidocs" \
    -H "Authorization: Bearer ${TOKEN}" \
    -H "Content-Type: application/json" \
    -d '{
      "title": "'"${TITLE}"'",
      "description": "'"${DESCRIPTION}"'",
      "anonAllowed": false,
      "imageUrl": "",
      "requireCallbackUrl": false,
      "categoryIds": [],
      "published": true,
      "apiProductName": "'"${PRODUCT_NAME}"'"
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

# Get the IDs of Products/Catalog items uploaded to the portal
RESPONSE_GET_CATELOG_ITEM=$(curl -s -X GET "https://apigee.googleapis.com/v1/organizations/${ORG}/sites/${ORG}-${PORTAL_URL}/apidocs" \
  -H "Authorization: Bearer ${TOKEN}")

apidocs=($(echo "$RESPONSE_GET_CATELOG_ITEM" | jq -r '.data[].id'))

# Read the YAML file and convert it to a byte array
byte_array=$(xxd -p "$YAML_FILE" | tr -d '\n' | sed 's/\(..\)/\\x\1/g')

# For each Product/Catalog item, upload the Open API Spec
for id in "${apidocs[@]}"; do
  RESPONSE_UPDATE_DOC=$(curl -s -o /dev/null -w "%{http_code}" -X GET "https://apigee.googleapis.com/v1/organizations/${ORG}/sites/${ORG}-${PORTAL_URL}/apidocs/${id}/documentation" \
    -H "Authorization: Bearer ${TOKEN}" \
    -H "Content-Type: application/json" \
    -d '{
      "oasDocumentation": {
          "spec": {
            "displayName": "D-TRO Open API Spec",
            "contents": "'"${byte_array}"'"
          }
        }
    }')
  echo "${RESPONSE_UPDATE_DOC}"
done