#!/bin/bash

# install.sh
# This script installs the simple-git-shell executable on a remote server.

# Function to install the simple-git-shell executable on a remote server
function install_git_shell() {
    local pathToExecutable=$1
    local destination=$2

    # Copy executable to the remote server
    echo "Copying simple-git-shell executable to the remote server..."
    scp -q "$pathToExecutable/simple-git-shell" "$destination:simple-git-shell"

    # Install and configure on the remote server
    ssh "$destination" "sudo -S mv -f simple-git-shell \"/usr/bin/simple-git-shell\" &&
               sudo chown root:root \"/usr/bin/simple-git-shell\" &&
               sudo chsh -s /usr/bin/simple-git-shell git"
}

# Prompt for input if not provided as command-line arguments
if [ -z $1 ]
then
    read -p "Please provide the path to the directory of the simple-git-shell executable: " pathToExecutable
else
    pathToExecutable=$1
fi

if [ -z $2 ]
then
    read -p "Please provide the destination (user@server): " destination
else
    destination=$2
fi

# Check if the provided directory exists
if [ ! -d "$pathToExecutable" ]; then
    echo "Error: The directory '$pathToExecutable' does not exist."
    exit 1
fi

# Check if the provided remote server destination is valid
if [[ ! "$destination" =~ ^[a-zA-Z0-9_.-]+@[a-zA-Z0-9_.-]+$ ]]; then
    echo "Error: Invalid destination format. Please provide a valid user@host format."
    exit 1
fi

# Call the installation function with validated inputs
install_git_shell "$pathToExecutable" "$destination"


echo "Installation completed successfully."