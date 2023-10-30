#!/bin/bash

# Script Description
# This script runs tests for a .NET application with options for coverage analysis, verbosity, and watching for changes.

# Set strict error handling
set -e

# Initialize variables and flags for optional arguments
coverage=false
report=false
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
  -c, --coverage  Perform coverage analysis.
  -r, --report    Create coverage report (only available
                  with coverage enabled).
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
        -r|--report)
            report=true
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

extraArgs=""

if [ "$watch" = true ]; then
    extraArgs="$extraArgs --"
fi

if [ "$verbose" = "debug" ] || [ "$verbose" = "d" ]; then
    extraArgs="$extraArgs --settings debug.runsettings"
elif [ "$verbose" = "quiet" ] || [ "$verbose" = "q" ]; then
    if [ "$watch" = true ]; then
        extraArgs="--quiet --nologo $extraArgs"
    else
        extraArgs="$extraArgs --verbosity quiet --nologo"
    fi
fi

if [ "$coverage" = true ]; then
    extraArgs="$extraArgs -p:ExcludeFromCoverage=/home/** -p:CollectCoverage=true -p:CoverletOutput=coverage/ -p:CoverletOutputFormat=opencover"
fi

if [ "$watch" = true ]; then
    dotnet watch test --project tests $extraArgs
else
    cd tests
    dotnet test $extraArgs
    cd ..
fi

if [ "$report" = true ]; then
    dotnet tool restore "-v" "q"
    dotnet tool run reportgenerator "-reports:tests/coverage/coverage.opencover.xml" "-targetdir:tests/coverage/report/" "-reporttypes:Html"
    open tests/coverage/report/index.html 1>/dev/null 2>&1
fi
