#!/bin/bash

WORKING_DIR=$1

for project in $(find $WORKING_DIR -name *.Tests.csproj); do 
    dotnet test $project /p:CollectCoverage=true /p:CoverletOutputFormat=lcov; 
done