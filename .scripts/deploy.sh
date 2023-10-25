#!/bin/bash

# deploy.sh
# This script deploys a .NET application and installs the simple-git-shell executable on a remote server.

# Usage: ./deploy.sh [-d destination]

# Default values for optional arguments
destination=""
runtime=""

# Function to handle errors and exit
handle_error() {
    echo $1
    echo "Deployment aborted."
    exit 1
}

# Parse optional arguments
while getopts "d:r:" opt; do
    case "$opt" in
        d) destination=$OPTARG ;;
        r) runtime=$OPTARG ;;
        \?) handle_error "Invalid option. Usage: $0 [-d destination]" ;;
    esac
done

# Run the deployment script and check for errors
$(dirname "$0")/publish.sh "$runtime"
if [ $? -ne 0 ]; then
    handle_error "Publishing script failed."
fi

# Run the installation script with provided arguments and check for errors
$(dirname "$0")/install.sh "bin/publish" "$destination"
if [ $? -ne 0 ]; then
    handle_error "Installation script failed."
fi

echo "Deployment completed successfully."
