FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /app
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# copy csproj and restore as distinct layers
COPY ./DfT.DTRO.csproj ./
RUN dotnet restore

# copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

# build common runtime image
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080
ENV FeatureManagement__DtroWrite=True \
    FeatureManagement__SchemasRead=True \
    FeatureManagement__SchemaWrite=True \
    FeatureManagement__DtroRead=True

ENTRYPOINT ["dotnet", "DfT.DTRO.dll"]

# # build image for Postgres migrations job
# FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:6.0-alpine AS postgres-migrations-job
# WORKDIR /app

# ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# COPY *.csproj ./
# RUN dotnet restore

# COPY . ./

# ENTRYPOINT ["/bin/sh", "-c", "dotnet tool restore && dotnet ef database update"]
