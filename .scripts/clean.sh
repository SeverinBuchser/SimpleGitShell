#!/bin/bash

find . -type d -wholename "./bin" -exec rm -rf {} +
find . -type d -wholename "./src/**/bin" -exec rm -rf {} +
find . -type d -wholename "./src/bin" -exec rm -rf {} +
find . -type d -wholename "./src/**/obj" -exec rm -rf {} +
find . -type d -wholename "./src/obj" -exec rm -rf {} +
find . -type d -wholename "./tests/**/bin" -exec rm -rf {} +
find . -type d -wholename "./tests/bin" -exec rm -rf {} +
find . -type d -wholename "./tests/**/obj" -exec rm -rf {} +
find . -type d -wholename "./tests/obj" -exec rm -rf {} +
