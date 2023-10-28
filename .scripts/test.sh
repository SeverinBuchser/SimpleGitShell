#!/bin/bash

# Script Description
# This script runs tests for a .NET application with options for coverage analysis, verbosity, and watching for changes.

# Set strict error handling
set -e

# Initialize variables and flags for optional arguments
extraArgs=""
coverage=false
watch=false
verbose="quiet"

# Function to handle errors and exit
handle_error() {
    echo "$1"
    echo "Testing aborted."
    exit 1
}

# Function to display usage instructions
show_usage() {
    cat <<EOM
Usage: $0 [options]

Options:
  -c, --coverage  Perform coverage analysis (Cannot be used with the watch option).
  -w, --watch     Start watching for changes.
  -v, --verbose   Set verbosity level:
                   - 'quiet' for quiet output (default).
                   - 'debug' for debug output.
  -h, --help      Show this usage information.

Examples:
  1. Run tests with coverage analysis:
     $0 -c

  2. Start watching for changes with debug output:
     $0 -w -v debug

  3. Execute tests quietly without coverage analysis:
     $0

EOM
    exit 1
}

# Parse optional arguments
while [[ $# -gt 0 ]]; do
    case "$1" in
        -c|--coverage)
            coverage=true
            ;;
        -w|--watch)
            watch=true
            ;;
        -v|--verbose)
            verbose=$2
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

# You can use the boolean variables and the verbosity level as needed in your script.
if [ "$coverage" = true ]; then
    extraArgs="-p:CollectCoverage=true -p:CoverletOutput=tests/TestResults/ -p:CoverletOutputFormat=opencover"
fi

if [ "$watch" = true ]; then
    if [ "$verbose" = "debug" ]; then
        extraArgs="$extraArgs --settings debug.runsettings"
    elif [ "$verbose" = "quiet" ]; then
        extraArgs="$extraArgs --quiet --no-logo"
    fi
fi

if [ "$watch" = true ] && [ "$coverage" = true ]; then
    echo "Warning: 'coverage' and 'watch' options cannot be used together. Coverage analysis will be disabled." >&2
    coverage=false
fi

if [ "$watch" = true ]; then
    dotnet watch test $extraArgs --project tests
else
    dotnet test $extraArgs
fi
