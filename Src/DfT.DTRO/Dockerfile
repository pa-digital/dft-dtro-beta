# Build stage
FROM --platform=amd64 mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /app

# Set environment variables
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the files and build the application
COPY . .
RUN dotnet publish -o out -c Release

# Runtime stage
FROM --platform=amd64 mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS runtime
WORKDIR /app

# Copy the output from the build stage
COPY --from=build /app/out .

# Expose the application port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://*:8080 \
    FeatureManagement__DtroWrite=True \
    FeatureManagement__SchemasRead=True \
    FeatureManagement__SchemaWrite=True \
    FeatureManagement__DtroRead=True

# Install EF Core tools
RUN dotnet tool install --global dotnet-ef --version 6.0.0

# To run migrations and start the application
ENTRYPOINT ["sh", "-c", "dotnet ef database update && ./DfT.DTRO"]
