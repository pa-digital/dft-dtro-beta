ENVIRONMENT=${1:-local}
echo "Environment: $ENVIRONMENT"

ENVIRONMENT="$ENVIRONMENT" dotnet test --logger "trx;LogFileName=test-results.trx"
