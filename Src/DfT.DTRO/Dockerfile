# Base image to build application
FROM --platform=amd64 mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /app
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# Copy the project file, restore dependencies,
# install global tool for migration
COPY *.csproj ./
RUN dotnet restore -r alpine-x64 && \
    dotnet tool install --global dotnet-ef --version 6.0.31

# Copy the rest of the files and build. Makes the startup script executable
COPY . .
RUN chmod +x /app/entrypoint.sh
RUN dotnet publish -o out -c Release -r alpine-x64 \
    --self-contained true /p:PublishReadyToRun=true /p:PublishSingleFile=true

# Base image to run application
FROM --platform=amd64 mcr.microsoft.com/dotnet/sdk:6.0-alpine AS runtime
WORKDIR /app

# Copy the published application, as well as the dotnet tools and startup script
COPY --from=build /app/out .
COPY --from=build /root/.dotnet /root/.dotnet
COPY --from=build /app/entrypoint.sh .
ENV PATH="$PATH:/root/.dotnet/tools"

# Expose the application port and set environment variables
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080 \
    FeatureManagement__DtroWrite=True \
    FeatureManagement__SchemasRead=True \
    FeatureManagement__SchemaWrite=True \
    FeatureManagement__DtroRead=True

# Run the application
ENTRYPOINT ["/app/entrypoint.sh"]

# # build image for Postgres migrations job
# FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:6.0-alpine AS postgres-migrations-job
# WORKDIR /app

# ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# COPY *.csproj ./
# RUN dotnet restore

# COPY . ./

# ENTRYPOINT ["/bin/sh", "-c", "dotnet tool restore && dotnet ef database update"]
