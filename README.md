# DfT D-TRO API Prototype (Beta)

A prototype implementation of API endpoints for publishing and consuming digital Traffic Regulation Orders.

## Developer Quickstart Instructions

The application can be run on Windows, MacOS and Linux.

### Windows
---

### MacOS
---

### Linux
---

<b>Note: you must ensure you install `.NET 6` as this is the version of `.NET` the application is built against.

#### Ubuntu

1. Install the .NET SDK and Runtime. https://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu-install?tabs=dotnet6&pivots=os-linux-ubuntu-2410 provides instructions on how to install the `.NET` SDK and Runtime for your distribution. Make sure to select the correct distribution and follow the instructions for `.NET 6`, as this is the version of `.NET` that the application is built against.

2. Install .NET Entity Framework with `dotnet tool install --global dotnet-ef --version 6.0.0`. Make sure to install version `6.0.0`.

3. Create the `.env` file for database credentials. Copy the file `dft-dtro-beta/Src/DfT.DTRO/docker/dev/.env.example` to `dft-dtro-beta/Src/DfT.DTRO/docker/dev/.env`. The username, password and database name values must match the values in `dft-dtro-beta/Src/DfT.DTRO/appsettings.json`

4. Install Docker. Follow the installation instructions at https://docs.docker.com/engine/install/ubuntu/ for installing Docker on your system.

5. Build and run the Docker containers. In `dft-dtro-beta/docker/dev` run `docker compose up`

6. Apply database migrations. In `dft-dtro-beta/Src/DfT.DTRO` run `dotnet ef update database`

7. Run the application. In `dft-dtro-beta/Src/DfT.DTRO` run `dotnet run`


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
