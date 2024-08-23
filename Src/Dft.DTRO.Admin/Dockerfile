FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /app

# Copy everything
COPY . ./

# Restore as distinct layers
RUN dotnet restore

# Build and publish a release
RUN dotnet publish -o out -c Release -r linux-musl-x64 \ 
    --self-contained true -p:PublishReadyToRun=true -p:PublishSingleFile=true

# Base image to run application
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS runtime
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/out .
RUN chmod +x Dft.DTRO.Admin

EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080

ENTRYPOINT ["./Dft.DTRO.Admin"]