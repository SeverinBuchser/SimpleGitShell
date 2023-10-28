#!/bin/bash

# Script Description
# This script deploys a .NET application and installs the simple-git-shell executable on a remote server.

# Set strict error handling
set -e

# Default values for optional arguments
destination=""
runtime=""
bump_version="patch"

# Function to handle errors and exit
handle_error() {
    echo "$1"
    echo "Deployment aborted."
    exit 1
}

# Function to display usage instructions
show_usage() {
    cat <<EOM
Usage: $0 [-d destination] [-r runtime] [-b version]

Options:
  -d, --destination  Set the deployment destination (e.g., user@server).
  -r, --runtime      Set the target runtime: linux-arm64 or linux-x64.
  -b, --bump-version Set the version bump type: major, minor, or patch.
  -h, --help         Show this usage information.

Examples:
  1. Deploy to a remote server with the default runtime and patch version bump:
     $0 -d myserver

  2. Deploy specifying the destination, runtime, and major version bump:
     $0 -d myserver -r linux-x64 -b major
EOM
    exit 1
}

# Parse optional arguments
while [[ $# -gt 0 ]]; do
    case "$1" in
        -d|--destination)
            destination=$2
            shift
            ;;
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

# If the destination is not provided, display an error message and exit
if [ -z "$destination" ]; then
    handle_error "Destination is required. Use the -d or --destination option."
fi

# Run the deployment script and check for errors
$(dirname "$0")/publish.sh "-r" "$runtime" "-b" "$bump_version"
if [ $? -ne 0 ]; then
    handle_error "Publishing script failed."
fi

# Run the installation script with provided arguments and check for errors
$(dirname "$0")/install.sh "bin/publish" "$destination"
if [ $? -ne 0 ]; then
    handle_error "Installation script failed."
fi

echo "Deployment completed successfully."
