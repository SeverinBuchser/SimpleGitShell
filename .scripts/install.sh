#!/bin/bash

# install.sh
# This script installs the git-shell executables on a remote server.

# Function to install the git-shell executables on a remote server
function install_git_shell() {
    local pathToExecutables=$1
    local destination=$2
    local gitShellDirectory=$3

    # Copy executables to the remote server
    echo "Copying Git Shell Executables to the remote server..."
    scp -rq "$pathToExecutables" "$destination:git-shell-commands"

    # Install and configure on the remote server
    ssh "$destination" "sudo -S cp -rf git-shell-commands/* \"$gitShellDirectory/git-shell-commands\" &&
               sudo chown -R git:git \"$gitShellDirectory/git-shell-commands\" &&
               sudo chsh -s $(which git-shell) git &&
               rm -rf git-shell-commands"
}

# Prompt for input if not provided as command-line arguments
if [ -z $1 ]
then
    read -p "Please provide the path to the directory of the executables: " pathToExecutables
else
    pathToExecutables=$1
fi

if [ -z $2 ]
then
    read -p "Please provide the destination (user@server): " destination
else
    destination=$2
fi

if [ -z $3 ]
then
    read -p $'Please provide the path to home directory of the git user (\"/path/to/git-home\"):\n' gitShellDirectory 
else
    gitShellDirectory=$3
fi

# Check if the provided directory exists
if [ ! -d "$pathToExecutables" ]; then
    echo "Error: The directory '$pathToExecutables' does not exist."
    exit 1
fi

# Check if the provided remote server destination is valid
if [[ ! "$destination" =~ ^[a-zA-Z0-9_.-]+@[a-zA-Z0-9_.-]+$ ]]; then
    echo "Error: Invalid destination format. Please provide a valid user@server format."
    exit 1
fi

# Call the installation function with validated inputs
install_git_shell "$pathToExecutables" "$destination" "$gitShellDirectory"


echo "Installation completed successfully."