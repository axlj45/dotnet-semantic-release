using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Utilities;

namespace AxlSoft.SemanticRelease.CommitAnalyzer
{
    // Pulled from https://daveaglick.com/posts/running-a-design-time-build-with-msbuild-apis
    public static class DotnetCoreBuildTools
    {
        public static string GetCoreBasePath(string projectPath)
        {
            // Ensure that we set the DOTNET_CLI_UI_LANGUAGE environment variable to "en-US" before
            // running 'dotnet --info'. Otherwise, we may get localized results.

            string originalCliLanguage = Environment.GetEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE");
            Environment.SetEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE", "en-US");

            try
            {
                // Create the process info
                ProcessStartInfo startInfo = new ProcessStartInfo("dotnet", "--info")
                {
                    // global.json may change the version, so need to set working directory
                    WorkingDirectory = Path.GetDirectoryName(projectPath),
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                // Execute the process
                using (Process process = Process.Start(startInfo))
                {
                    List<string> lines = new List<string>();
                    process.OutputDataReceived += (_, e) =>
                    {
                        if (!string.IsNullOrWhiteSpace(e.Data))
                        {
                            lines.Add(e.Data);
                        }
                    };
                    process.BeginOutputReadLine();
                    process.WaitForExit();
                    return ParseCoreBasePath(lines);
                }
            }
            finally
            {
                Environment.SetEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE", originalCliLanguage);
            }
        }

        public static string ParseCoreBasePath(List<string> lines)
        {
            if (lines == null || lines.Count == 0)
            {
                throw new Exception("Could not get results from `dotnet --info` call");
            }

            foreach (string line in lines)
            {
                int colonIndex = line.IndexOf(':');
                if (colonIndex >= 0
                    && line.Substring(0, colonIndex).Trim().Equals("Base Path", StringComparison.OrdinalIgnoreCase))
                {
                    return line.Substring(colonIndex + 1).Trim();
                }
            }

            throw new Exception("Could not locate base path in `dotnet --info` results");
        }

        public static Dictionary<string, string> GetCoreGlobalProperties(string projectPath, string toolsPath)
        {
            string solutionDir = Path.GetDirectoryName(projectPath);
            string extensionsPath = toolsPath;
            string sdksPath = Path.Combine(toolsPath, "Sdks");
            string roslynTargetsPath = Path.Combine(toolsPath, "Roslyn");

            return new Dictionary<string, string>
            {
                { "SolutionDir", solutionDir },
                { "MSBuildExtensionsPath", extensionsPath },
                { "MSBuildSDKsPath", sdksPath },
                { "RoslynTargetsPath", roslynTargetsPath }
            };
        }


        public static Project GetCoreProject(string projectPath)
        {
            string toolsPath = GetCoreBasePath(projectPath);
            Dictionary<string, string> globalProperties = GetCoreGlobalProperties(projectPath, toolsPath);
            ProjectCollection projectCollection = new ProjectCollection(globalProperties);
            projectCollection.AddToolset(new Toolset(ToolLocationHelper.CurrentToolsVersion, toolsPath, projectCollection, string.Empty));

            Environment.SetEnvironmentVariable("MSBuildExtensionsPath", globalProperties["MSBuildExtensionsPath"]);
            Environment.SetEnvironmentVariable("MSBuildSDKsPath", globalProperties["MSBuildSDKsPath"]);

            Project project = projectCollection.LoadProject(projectPath);
            return project;
        }
    }
}