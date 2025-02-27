#!/bin/bash

pushd Src/DfT.DTRO
dotnet ef database update
dotnet run &
echo "Waiting for https://0.0.0.0:5001/index.html to be up..."
for i in {1..60}; do
curl -k --output /dev/null --silent --head --fail https://0.0.0.0:5001/index.html && break
echo "Still waiting..."
sleep 5
done
echo "Server is up!"

popd
./run_integration_tests.sh