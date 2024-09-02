#!/bin/bash

# Script Variables
ORG=$apigee_organisation
TOKEN=$1

# Make the API call to retrieve developer's details
RESPONSE=$(curl -s -o /dev/null -X GET "https://apigee.googleapis.com/v1/organizations/${ORG}/developers/vighnesh.pindoria@dft.gov.uk/apps/ACME-Publisher-Application" \
  -H "Authorization: Bearer ${TOKEN}" \
  -H "Content-Type: application/json")
echo RESPONSE

consumerKey=$(echo RESPONSE | jq -r '.credentials[0].consumerKey')
apiproduct=$(echo RESPONSE | jq -r '.credentials[0].apiProducts[0].apiproduct')

# Make the API call approve api product
RESPONSE=$(curl -s -o /dev/null -X POST "https://apigee.googleapis.com/v1/organizations/${ORG}/developers/vighnesh.pindoria@dft.gov.uk/apps/ACME-Publisher-Application/keys/${consumerKey}/apiproducts/${apiproduct}?action=approve" \
  -H "Authorization: Bearer ${TOKEN}" \
  -H "Content-Type: application/json")
echo RESPONSE
