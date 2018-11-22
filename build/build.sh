#!/bin/bash

WORKING_DIR=$1
SRC_DIR=$WORKING_DIR/src
PKG_DIR=$WORKING_DIR/packages

mkdir -p $PKG_DIR
rm -rf $PKG_DIR/*.nupkg

dotnet pack --configuration=release $SRC_DIR/SemanticRelease.Extensibility/*.csproj
dotnet pack --configuration=release $SRC_DIR/SemanticRelease.CommitAnalyzer/*.csproj
dotnet pack --configuration=release $SRC_DIR/SemanticRelease.Core/*.csproj
dotnet build --configuration=release $SRC_DIR/SemanticRelease.Tool/*.csproj
cp $SRC_DIR/**/bin/release/*.nupkg $PKG_DIR

dotnet build --configuration=release $SRC_DIR/SemanticRelease.GlobalTool/*.csproj
cp $SRC_DIR/SemanticRelease.GlobalTool/bin/release/*.nupkg $PKG_DIR


# dotnet tool uninstall --global SemanticRelease.GlobalTool
# dotnet tool install --global SemanticRelease.GlobalTool
