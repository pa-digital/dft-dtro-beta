#!/bin/bash

LINE_THRESHOLD="44.87"
PROJECT_NAME="Dft.DTRO.Tests"
 
dotnet tool install -g dotnet-reportgenerator-globaltool
  
dotnet test Src/$PROJECT_NAME/$PROJECT_NAME.csproj --verbosity normal \
/p:CollectCoverage=true \
/p:ExcludeByFile=**/*Migrations/*.cs \
/p:Threshold=$LINE_THRESHOLD /p:ThresholdType=line \
/p:CoverletOutput=Coverage/coverage.json \
/p:CoverletOutputFormat=opencover

if [ $? -ne 0 ]; then
  echo "Tests failed or coverage threshold not met."
  exit 1
fi

reportgenerator -reports:Src/$PROJECT_NAME/Coverage/coverage.json -targetdir:TestReports/UnitTests -reporttypes:Html
