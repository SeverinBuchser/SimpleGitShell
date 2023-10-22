#!/bin/bash

# Usage: ./deploy.sh
echo "This script runs tests and deploys a .NET application."

# Set strict error handling
set -e

# Run Tests
echo "Running tests..."
dotnet test --nologo --verbosity quiet
if [ $? -ne 0 ]; then
    echo "Tests failed. Aborting Publishing."
    exit 1
fi

# Prompt for runtime
read -p "Please provide the runtime of the host (Valid runtime options: linux-arm64, linux-x64): " runtime

case $runtime in
    linux-arm64|linux-x64)
        # Valid runtime
        ;;
    *)
        echo "Invalid runtime option. Valid options are: linux-arm64, linux-x64. Aborting Publishing."
        exit 1
        ;;
esac

# Publishing
echo "Publishing Git Shell Executables..."
dotnet publish -r $runtime --self-contained --nologo --verbosity quiet

echo "Publishing completed successfully."
