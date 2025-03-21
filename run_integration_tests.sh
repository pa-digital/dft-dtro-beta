ENVIRONMENT=${1:-local}
echo "Environment: $ENVIRONMENT"

cd Src/DfT.DTRO.IntegrationTests
dotnet build
ENVIRONMENT="$ENVIRONMENT" dotnet test --logger "trx;LogFileName=test-results.trx"
