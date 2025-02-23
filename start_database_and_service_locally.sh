#!/bin/bash

# Start Docker Compose in a new terminal
if [[ "$OSTYPE" == "darwin"* ]]; then
    # macOS (using Apple Terminal)
    osascript -e 'tell application "Terminal" to do script "cd $(pwd)/docker/dev && sudo docker compose up"'
else
    # Linux (using gnome-terminal)
    gnome-terminal -- bash -c "cd $(pwd)/docker/dev && sudo docker compose up; exec bash"
fi

# Wait for Docker to initialize
for i in {1..10}; do
    if pg_isready -h localhost -p 5432 -U postgres; then
        echo "PostgreSQL is ready"
        break
    fi
    echo "Waiting for PostgreSQL..."
    sleep 5
done

# Open a new terminal to run dotnet commands
if [[ "$OSTYPE" == "darwin"* ]]; then
    osascript -e 'tell application "Terminal" to do script "cd $(pwd)/Src/DfT.DTRO && dotnet ef database update && dotnet run"'
else
    gnome-terminal -- bash -c "cd $(pwd)/Src/DfT.DTRO && dotnet ef database update && dotnet run; exec bash"
fi