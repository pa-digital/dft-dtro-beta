To execute the integration tests against localhost, first ensure the database is up, and the app is running; then run one of the following commands from the project root:

```
# Linux / Mac:
./run_integration_tests.sh

# Windows:
powershell -File ".\run_integration_tests.ps1"
```

You can view the test results at Src/DfT.DTRO.IntegrationTests/TestResults/test-results.trx (in Visual Studio only).
