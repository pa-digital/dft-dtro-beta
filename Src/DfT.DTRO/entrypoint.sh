#!/bin/sh

# Run the database migration
dotnet ef database update

# Run the application
./DfT.DTRO
