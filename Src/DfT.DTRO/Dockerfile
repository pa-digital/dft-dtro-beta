# Base image to build application
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /app
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# Copy the project file and restore dependencies,
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the files and build
COPY . .
RUN dotnet publish -o out -c Release \
    --self-contained true /p:PublishReadyToRun=true /p:PublishSingleFile=true

# Base image to run application
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS runtime
WORKDIR /app

# Copy the published application as well as the dotnet tools and startup
COPY --from=build /app/out .

# Expose the application port and set environment variables
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080 \
    FeatureManagement__DtroWrite=True \
    FeatureManagement__SchemasRead=True \
    FeatureManagement__SchemaWrite=True \
    FeatureManagement__DtroRead=True

# Run the application
ENTRYPOINT ["./DfT.DTRO"]
