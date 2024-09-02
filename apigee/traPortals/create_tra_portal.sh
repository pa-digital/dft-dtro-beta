#!/bin/bash

# Script Variables
ORG=$apigee_organisation
TOKEN=$1

# Convert comma-delimited string into an array
if [[ "$tra" == *","* ]]; then
  IFS=',' read -r -a tra_array <<< "$tra"
else
  tra_array=("$tra")
fi

# Loop through each element in the array
for tra_element in "${tra_array[@]}"; do

  # Make the API call
  RESPONSE=$(curl -s -o /dev/null -w "%{http_code}" -X POST "https://apigee.googleapis.com/v1/organizations/${ORG}/sites" \
    -H "Authorization: Bearer ${TOKEN}" \
    -H "Content-Type: application/json" \
    -d '{
      "name": "'"${tra_element^} D-TRO Portal"'",
      "description": "'"This is the ${tra_element^} Portal for D-TRO"'"
    }')

  # Error checking and handling
  if [ "$RESPONSE" -eq 200 ]; then
    echo "${tra_element^} Portal successfully created in ${ORG}."
  elif [ "$RESPONSE" -eq 409 ]; then
    echo "${tra_element^} Portal already exists in ${ORG}."
  else
    echo "Failed to create ${tra_element^} portal in ${ORG}. HTTP response code: $RESPONSE"
  fi
done
