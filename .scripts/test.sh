#!/bin/bash

# Initialize extra args
extraArgs=""

# Initialize boolean variables
coverage=false
watch=false
debug=false

# Define a function to show the usage instructions
show_usage() {
  echo "Usage: $0 [options]"
  echo "Options:"
  echo "  -c, --coverage  Perform coverage analysis. Cannot be used with the watch option"
  echo "  -w, --watch     Start watching for changes"
  echo "  -d, --debug     Enable debugging"
  exit 1
}

# Process command-line options
while [[ $# -gt 0 ]]; do
    case "$1" in
    -c|--coverage)
        coverage=true
        ;;
    -w|--watch)
        watch=true
        ;;
    -d|--debug)
        debug=true
        ;;
    *)
        echo "Invalid option: $1"
        show_usage
        ;;
    esac
    shift
done

# You can use the boolean variables as needed in your script.
if $coverage; then
    extraArgs="-p:CollectCoverage=true -p:CoverletOutput=tests/TestResults/ -p:CoverletOutputFormat=opencover"
fi

if $debug && $watch; then
    extraArgs="$extraArgs --settings debug.runsettings"
elif $debug; then
    extraArgs="$extraArgs --settings tests/debug.runsettings"
fi

if $watch && $coverage; then
    show_usage
fi



if $watch;
then
    dotnet watch test $extraArgs --project tests 
else
    dotnet test $extraArgs 
fi