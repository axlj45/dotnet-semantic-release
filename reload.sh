mkdir -p ./packages
rm -rf ./packages/*.nupkg

dotnet pack --configuration=release src/SemanticRelease.Extensibility/*.csproj
dotnet pack --configuration=release src/SemanticRelease.CommitAnalyzer/*.csproj
dotnet pack --configuration=release src/SemanticRelease.Core/*.csproj
dotnet build --configuration=release src/SemanticRelease.Tool/*.csproj
cp src/**/bin/Release/*.nupkg ./packages

dotnet build --configuration=release src/SemanticRelease.GlobalTool/*.csproj
cp src/SemanticRelease.GlobalTool/bin/Release/*.nupkg ./packages


# dotnet tool uninstall --global SemanticRelease.GlobalTool
# dotnet tool install --global SemanticRelease.GlobalTool
