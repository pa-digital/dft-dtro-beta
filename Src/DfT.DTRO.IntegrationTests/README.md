To execute the integration tests locally against localhost:
1. Ensure the database is up, with connection string set up correcly in `docker/dev/.env`.
2. Ensure the the app is running.
3. Run one of the following commands from the project root:

```
# Linux / Mac:
./run_integration_tests.sh

# Windows:
dotnet test --logger "html;LogFileName=test-results.html"
```

You can view the test results at Src/DfT.DTRO.IntegrationTests/TestResults/test-results.html
