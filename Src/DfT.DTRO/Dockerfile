FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /app
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore -r alpine-x64 /p:PublishReadyToRun=true

# copy everything else and build
COPY . .
RUN dotnet publish --framework net6.0 -o out -c Release -r alpine-x64 \
    --self-contained true /p:PublishReadyToRun=true /p:PublishSingleFile=true

# build common runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080
ENV FeatureManagement__DtroWrite=True \
    FeatureManagement__SchemasRead=True \
    FeatureManagement__SchemaWrite=True \
    FeatureManagement__DtroRead=True

ENTRYPOINT ["./DfT.DTRO"]

# # build image for Postgres migrations job
# FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:6.0-alpine AS postgres-migrations-job
# WORKDIR /app

# ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# COPY *.csproj ./
# RUN dotnet restore

# COPY . ./

# ENTRYPOINT ["/bin/sh", "-c", "dotnet tool restore && dotnet ef database update"]
