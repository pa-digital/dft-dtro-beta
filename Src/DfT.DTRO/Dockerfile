# Base image to build application
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /app
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1

# Copy the project file and restore dependencies,
COPY *.csproj ./
RUN dotnet restore -r linux-musl-x64

# Copy the rest of the files and build
COPY . .
RUN dotnet publish -o out -c Release -r linux-musl-x64 \
    --self-contained true -p:PublishReadyToRun=true -p:PublishSingleFile=true

# Create the migration bundle
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS migration-bundle
WORKDIR /app
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1

# Copy the project file and create the migration bundle
COPY *.csproj ./
RUN dotnet tool install --global dotnet-ef --version 6.0.32
RUN dotnet restore

# Update PATH for the current shell session
ENV PATH="$PATH:/root/.dotnet/tools"

COPY . ./
RUN dotnet ef migrations bundle --self-contained -r linux-musl-x64

# Base image to run application
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS runtime
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/out .
COPY --from=migration-bundle /app/efbundle .
RUN chmod +x efbundle
RUN chmod +x DfT.DTRO

# Expose the application port and set environment variables
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080

CMD ["/bin/sh", "-c", "./efbundle && ./DfT.DTRO"]
