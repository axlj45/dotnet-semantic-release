dotnet build **/*.sln
dotnet tool uninstall --global SemanticRelease.GlobalTool
dotnet tool install --global SemanticRelease.GlobalTool
