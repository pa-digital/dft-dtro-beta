# DfT DTRO API prototype (Alpha)

A prototype implementation of API endpoints for publishing and consuming digital Traffic Regulation Orders.

## Run

To run the solution on Linux/OS X, use the following commands.
```
make docker-build
make docker-run
```

If you are running on a Windows host, please see the contents of the Makefile in the directory root for individual docker commands to build, run and launch the service.

## GCP Components

To deploy this project, the following GCP APIs need to be enabled at a project level:

1. Cloud Domains API
1. Cloud DNS API
1. Firestore API
1. Compute Engine API
1. Artifact Registry API
1. Cloud Run API
1. Cloud Resource Manager API
1. Identity and Access Management (IAM) API
1. Service Usage API

## Terraform Use

To support account switching (but with single variable files), when initialising terraform you must first make sure that the DEPLOY_ENVIRONMENT environment variable has been set. For the development GCP environment, this should be set to "default". For deployment to a release environment, this should be set to "dft-gcp". Terraform code has been configured to reconcile the appropriate variables by drawing these from the terraform/configuration/environments/${DEPLOY_ENVIRONMENT}.tfvars file.

As an applied example, the following commands will initialise terraform and select the workspace for the development account:

```
export DEPLOY_ENVIRONMENT=default
make init
```

To simplify/automate the initialisation and selection of workspaces - make sure to the `make` commands in the terraform directory.

## Sample API requests

Sample requests can be found in the src/DfT.DTRO/Schemas directory.
