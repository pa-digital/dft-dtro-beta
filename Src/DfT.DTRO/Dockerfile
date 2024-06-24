FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build-env
WORKDIR /app

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build
COPY . ./
RUN dotnet restore -r alpine-x64 /p:PublishReadyToRun=true
RUN dotnet publish --framework net6.0 -o out -c Release -r alpine-x64 --self-contained true /p:PublishReadyToRun=true /p:PublishSingleFile=true

# build common runtime image
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS runtime
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080

# build runtime image for publish service
FROM runtime AS publish-service
ENV FeatureManagement__DtroWrite=True
ENV FeatureManagement__SchemasRead=True
ENV FeatureManagement__SchemaWrite=True

ENTRYPOINT ["./DfT.DTRO"]

# build runtime image for search service
FROM runtime AS search-service
WORKDIR /app
COPY --from=build-env /app/out .

ENV FeatureManagement__DtroRead=True

ENTRYPOINT ["./DfT.DTRO"]

# # build image for Postgres migrations job
# FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:6.0-alpine AS postgres-migrations-job
# WORKDIR /app

# ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# COPY *.csproj ./
# RUN dotnet restore

# COPY . ./

# ENTRYPOINT ["/bin/sh", "-c", "dotnet tool restore && dotnet ef database update"]
