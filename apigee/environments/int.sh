#!/bin/bash

echo "env=test" >> $GITHUB_ENV
echo "env_name_prefix=int" >> $GITHUB_ENV
echo "workload_identity_service_account=${service_account}" >> $GITHUB_ENV
echo "apigee_organisation=${apigee_organisation}" >> $GITHUB_ENV
echo "target_url=http://7.0.0.3" >> $GITHUB_ENV