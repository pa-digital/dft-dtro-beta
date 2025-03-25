# Summary
The integration tests are API tests which test the solution as a black box, sending requests as publishers and consumers.

# Running the tests
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

# Debugging
If a test is failing on a particular HTTP request and you require further information (for example, if a request is returning an internal server error with no body), do the following:
1. Put a breakpoint in the test code at a known failure point.
2. Run the test in debug mode as far as the breakpoint.
3. Add a second breakpoint to Src/DfT.DTRO.IntegrationTests/IntegrationTests/Helpers/HttpRequestHelper.cs just after the comment "Put breakpoint here to read curlString to debug a request".
4. Continue test execution to the second breakpoint.
5. Copy the value of `curlString`.
6. Add a third breakpoint to a relevant line in the production code.
7. Execute the copied curl.
8. Step through the code from the third breakpoint.