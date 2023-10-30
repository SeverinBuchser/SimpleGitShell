#!/bin/bash

# Check if the correct number of arguments are provided
if [ "$#" -ne 2 ]; then
    echo "Usage: $0 <type> <project_file>"
    echo "  <type>: major, minor, or patch"
    echo "  <project_file>: Path to the project file containing the version tag"
    exit 1
fi

# Validate the type argument
type="$1"
if [ "$type" != "major" ] && [ "$type" != "minor" ] && [ "$type" != "patch" ]; then
    echo "Invalid type. Use 'major', 'minor', or 'patch'."
    exit 1
fi

# Validate the project file argument
project_file="$2"
if [ ! -f "$project_file" ]; then
    echo "Project file does not exist: $project_file"
    exit 1
fi

# Get old Version and extract major, minor, and patch
old_version=$(grep -oP '<Version>\K\d+\.\d+\.\d+(?=<\/Version>)' "$project_file")
IFS='.' read -r major_version minor_version patch_version <<< "$old_version"

# Bump the selected version component
case "$type" in
    "major")
        major_version=$((major_version + 1))
        minor_version=0
        patch_version=0
        ;;
    "minor")
        minor_version=$((minor_version + 1))
        patch_version=0
        ;;
    "patch")
        patch_version=$((patch_version + 1))
        ;;
esac

# Create the new version string
new_version="$major_version.$minor_version.$patch_version"

# Use sed to replace the version in the project file
sed -i -E "s/<Version>[0-9]+\.[0-9]+\.[0-9]+<\/Version>/<Version>$new_version<\/Version>/g" "$project_file"

# Display the updated version
echo "Version bumped from $old_version to $new_version"
