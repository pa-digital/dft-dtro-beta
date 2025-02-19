ENVIRONMENT=${1:-local}
echo "Environment: $ENVIRONMENT"

ENVIRONMENT="$ENVIRONMENT" dotnet test
