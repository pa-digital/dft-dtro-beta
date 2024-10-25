#!/bin/bash

# Script Variables
ORG=$apigee_organisation
YAML_FILE="openApi/openapi3_0.yaml"
PUBLISHER_YAML_FILE="openApi/apiDoc/$env_name_prefix/openapi3_0-publisher.yaml"
CONSUMER_YAML_FILE="openApi/apiDoc/$env_name_prefix/openapi3_0-consumer.yaml"

to_title_case() {
  echo "$1" | sed -e 's/\b./\u&/g' -e 's/-/ /g'
}
TITLE_ENV=$(to_title_case "${env}")
PORTAL_NAME="${TITLE_ENV} Developer D-TRO Portal"
PORTAL_URL="$(echo "${PORTAL_NAME//[- ]/}" | tr '[:upper:]' '[:lower:]')"

TOKEN=$1

# List of product names
PRODUCT_NAMES=("central-service-operator" "publisher" "consumer")

# Loop through each product name
for PRODUCT in "${PRODUCT_NAMES[@]}"; do

  # Name of product created via product.json
  PRODUCT_NAME="${env}-${PRODUCT}"

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
      "anonAllowed": true,
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

# Get the titles and IDs of the Products/Catalog items uploaded earlier to the portal and persist them to a map
RESPONSE_GET_CATELOG_ITEM=$(curl -s -X GET "https://apigee.googleapis.com/v1/organizations/${ORG}/sites/${ORG}-${PORTAL_URL}/apidocs" \
  -H "Authorization: Bearer ${TOKEN}")

declare -A apidocs
while IFS="=" read -r title id; do
  apidocs["$title"]="$id"
done < <((echo "$RESPONSE_GET_CATELOG_ITEM" | jq -r '.data[] | "\(.title)=\(.id)"'))

# Read the appropriate YAML file and convert it to a base64 string with no wrap around
if [[ "$env" == "dev" || "$env" == "test" ]]; then
  base64_string=$(base64 -w 0 "$YAML_FILE")
elif [ "$PRODUCT" = "publisher" ]; then
  base64_string=$(base64 -w 0 "$PUBLISHER_YAML_FILE")
else
  base64_string=$(base64 -w 0 "$CONSUMER_YAML_FILE")
fi

# For each Product/Catalog item, upload the Open API Spec
for title in "${!apidocs[@]}"; do
  RESPONSE_UPDATE_DOC=$(curl -s -o /dev/null -w "%{http_code}" -X PATCH "https://apigee.googleapis.com/v1/organizations/${ORG}/sites/${ORG}-${PORTAL_URL}/apidocs/${apidocs[$title]}/documentation" \
    -H "Authorization: Bearer ${TOKEN}" \
    -H "Content-Type: application/json" \
    -d '{
      "oasDocumentation": {
          "spec": {
            "displayName": "'"${title} Open API Spec"'",
            "contents": "'"${base64_string}"'"
          }
        }
    }')

  # Error checking and handling
  if [ "$RESPONSE_UPDATE_DOC" -eq 200 ]; then
    echo "${title} Open API Spec successfully uploaded."
  else
    echo "Failed to upload ${title} Open API Spec. HTTP response code: $RESPONSE_UPDATE_DOC"
    exit 1
  fi
done
