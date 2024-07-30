# Base image to build UI application
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /app

# Copy the project file and restore dependencies,
COPY *.csproj ./
RUN dotnet restore -r linux-musl-x64

# Copy the rest of the files and build
COPY *.csproj ./

RUN dotnet publish -o out -c Release -r linux-musl-x64 \
    --self-contained true -p:PublishReadyToRun=true -p:PublishSingleFile=true

# Base image to run application
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS runtime
WORKDIR /app

# Expose the application port and set environment variables
EXPOSE 80
ENV ASPNETCORE_URLS=http://*:80 \
    BASE_URL=http://localhost:5001

ENTRYPOINT ["./Dft.DTRO.Admin"]