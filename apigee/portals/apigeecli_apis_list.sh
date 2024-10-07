#!/bin/bash

# Script Variables
ORG=$apigee_organisation
TOKEN=$1

ORIGINAL_RESPONSE=$(apigeecli apis list --incl --token ${TOKEN} --org ${ORG} )
echo "ORIGINAL_RESPONSE"
echo "$ORIGINAL_RESPONSE"

JQ_RESPONSE=$(apigeecli apis list --incl --token ${TOKEN} --org ${ORG} | jq . )
echo "JQ_RESPONSE_RESPONSE"
echo "$JQ_RESPONSE_RESPONSE"

RESPONSE=$(apigeecli apis list --incl --token ${TOKEN} --org ${ORG})
if echo "$RESPONSE" | jq . >/dev/null 2>&1; then
    # If valid JSON, proceed with the jq query
    revision_list=($(echo "$output" | jq -r --arg proxy_name "dtro-dev-server" '.proxies[] | select(.name == $proxy_name) | .revision[]'))
else
    echo "Error: Received non-JSON output"
    echo "$RESPONSE"
fi