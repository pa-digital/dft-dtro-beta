#!/bin/bash
 
dotnet tool install -g dotnet-reportgenerator-globaltool
  
dotnet test Src/Dft.DTRO.Tests/Dft.DTRO.Tests.csproj \
/p:CollectCoverage=true \
/p:Include="[DfT.DTRO]*" \
/p:Threshold=21.37 /p:ThresholdType=line \
/p:Threshold=31.63 /p:ThresholdType=branch \
/p:Threshold=35.02 /p:ThresholdType=method \
/p:CoverletOutput=Coverage/coverage.json \
/p:CoverletOutputFormat=opencover && \
reportgenerator -reports:Src/Dft.DTRO.Tests/Coverage/coverage.json -targetdir:TestReports/UnitTests -reporttypes:Html
