#!/bin/bash

# Script Description
# This script runs tests and publishes a .NET application.

# Set strict error handling
set -e

# Default values for optional arguments
bump_version="patch"
runtime=""

# Function to handle errors and exit
handle_error() {
    echo "$1"
    echo "Publishing aborted."
    exit 1
}

# Function to display usage instructions
show_usage() {
    cat <<EOM
Usage: $0 [-b bump-version] [-r runtime]

Options:
  -r, --runtime         Set the target runtime: linux-arm64 or linux-x64.
  -b, --bump-version    Set the type of version bump: major, minor, patch or nobump (default: nobump).
  -h, --help            Show this usage information.

Examples:
  1. Publish with a minor version bump and target runtime linux-x64:
     $0 -b minor -r linux-x64

  2. Publish with the default options (patch bump and prompt for runtime):
     $0

EOM
    exit 1
}

# Parse optional arguments
while [[ $# -gt 0 ]]; do
    case "$1" in
        -r|--runtime)
            runtime=$2
            shift
            ;;
        -b|--bump-version)
            bump_version=$2
            shift
            ;;
        -h|--help)
            show_usage
            ;;
        *)
            handle_error "Invalid option or argument: $1. Use -h or --help for usage information."
            ;;
    esac
    shift
done


# If runtime is not provided, prompt for it
if [ -z "$runtime" ]; then
    read -p "Please provide the runtime of the host (Valid runtime options: linux-arm64, linux-x64): " runtime
fi

# Validate runtime
case $runtime in
    linux-arm64|linux-x64)
        # Valid runtime
        ;;
    *)
        handle_error "Invalid runtime option. Valid options are: linux-arm64, linux-x64. Aborting Publishing."
        ;;
esac

# Version bump
if [ $bump_version != "nobump" ]; then
    echo "Bumping the version..."
    $(dirname "$0")/bump-version.sh "$bump_version" "src/simple-git-shell.csproj"
    if [ $? -ne 0 ]; then
        handle_error "Version Bump script failed."
    fi
    echo "Version bumped successfully."
fi

# Run tests
echo "Running tests..."
dotnet test --nologo --verbosity quiet
if [ $? -ne 0 ]; then
    handle_error "Version Bump script failed."
fi
echo "Tests completed successfully."

# Publishing
echo "Publishing Git Shell Executable..."
dotnet publish -r $runtime --self-contained --nologo --verbosity quiet
if [ $? -ne 0 ]; then
    handle_error "Publishing failed."
fi
echo "Publishing completed successfully."
