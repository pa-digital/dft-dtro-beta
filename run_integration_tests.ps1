# Set the environment variable to the first argument or default to "local"
$ENVIRONMENT = if ($args.Count -gt 0) { $args[0] } else { "local" }
Write-Host "Environment: $ENVIRONMENT"

# Change directory
Set-Location Src/DfT.DTRO.IntegrationTests

# Run the dotnet test command with the environment variable
$env:ENVIRONMENT = $ENVIRONMENT
dotnet test --logger "trx;LogFileName=test-results.trx"
