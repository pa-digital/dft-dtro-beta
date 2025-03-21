#!/bin/bash

LINE_THRESHOLD="43.82"
PROJECT_NAME="Dft.DTRO.Tests"
 
dotnet tool install -g dotnet-reportgenerator-globaltool
  
dotnet test Src/$PROJECT_NAME/$PROJECT_NAME.csproj --verbosity normal \
/p:CollectCoverage=true \
/p:ExcludeByFile=**/*Migrations/*.cs \
/p:Threshold=$LINE_THRESHOLD /p:ThresholdType=line \
/p:CoverletOutput=Coverage/coverage.json \
/p:CoverletOutputFormat=opencover

reportgenerator -reports:Src/$PROJECT_NAME/Coverage/coverage.json -targetdir:TestReports/UnitTests -reporttypes:Html
