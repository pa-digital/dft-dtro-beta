# Base image to build application
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /app
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# Copy the project file and restore dependencies,
COPY *.csproj ./
RUN dotnet restore -r alpine-x64

# Copy the rest of the files and build
COPY . .
RUN dotnet publish -o out -c Release -r alpine-x64 \
    --self-contained true /p:PublishReadyToRun=true /p:PublishSingleFile=true

# Base image to run application
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS runtime
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/out .

# Expose the application port and set environment variables
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080 \
    FeatureManagement__DtroWrite=True \
    FeatureManagement__SchemasRead=True \
    FeatureManagement__SchemaWrite=True \
    FeatureManagement__DtroRead=True

#Add PostgreSQL 'D-TRO_Database.sql' script
COPY Scripts/D-TRO_Database.sql /app
RUN apk add postgresql-client
RUN chmod +x /app/D-TRO_Database.sql
RUN psql -h 10.147.0.2 -U postgres -d DTRO -f /app/D-TRO_Database.sql


# Run PostgreSQL 'D-TRO_Database.sql' script
ENV POSTGRES_USER=postgres
ENV POSTGRES_PASSWORD=admin
ENV POSTGRES_DB=DTRO
CMD ["postgres"]
RUN /app/D-TRO_Database.sql

# Run the application
ENTRYPOINT ["./DfT.DTRO"]
