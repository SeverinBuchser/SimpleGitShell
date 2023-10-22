#!/bin/bash

# deploy.sh
# This script deploys a .NET application and installs git-shell executables on a remote server.

# Usage: ./deploy.sh [-d destination] [-g gitShellDirectory]

# Default values for optional arguments
pathToExecutables=""
destination=""
gitShellDirectory=""

# Function to handle errors and exit
handle_error() {
    echo "Deployment aborted."
    exit 1
}

# Parse optional arguments
while getopts "d:g:" opt; do
    case "$opt" in
        d) destination=$OPTARG ;;
        g) gitShellDirectory=$OPTARG ;;
        \?) handle_error "Invalid option. Usage: $0 [-d destination] [-g gitShellDirectory]" ;;
    esac
done

# Run the deployment script and check for errors
$(dirname "$0")/publish.sh
if [ $? -ne 0 ]; then
    handle_error "Publishing script failed."
fi

# Run the installation script with provided arguments and check for errors
$(dirname "$0")/install.sh "bin/publish/commands" "$destination" "$gitShellDirectory"
if [ $? -ne 0 ]; then
    handle_error "Installation script failed."
fi

echo "Deployment and installation completed successfully."
