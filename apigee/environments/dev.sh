#!/bin/bash

echo "dev"
echo "env=dev" >> $GITHUB_ENV
echo "env_name_prefix=dev" >> $GITHUB_ENV
echo "gcp_project=dft-dtro-dev-01" >> $GITHUB_ENV
echo "service=${service}" >> $GITHUB_ENV
echo "workload_identity_service_account=${service_account}" >> $GITHUB_ENV
echo "apigee_organisation=${apigee_organisation}" >> $GITHUB_ENV
echo "target_url=http://7.0.8.2" >> $GITHUB_ENV
echo "trusted_domain=https://dtro-dev.dft.gov.uk" >> $GITHUB_ENV