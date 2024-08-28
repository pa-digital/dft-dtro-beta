#!/bin/bash

echo "prod"
echo "env=prod" >> $GITHUB_ENV
echo "env_name_prefix=prod" >> $GITHUB_ENV
echo "tra=${tra}" >> $GITHUB_ENV
echo "workload_identity_service_account=${service_account}" >> $GITHUB_ENV
echo "apigee_organisation=${apigee_organisation}" >> $GITHUB_ENV