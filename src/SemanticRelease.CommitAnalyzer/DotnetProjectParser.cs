using System;
using System.IO.Abstractions;
using System.Linq;
using Microsoft.Build.Evaluation;
using SemanticRelease.Extensibility;

namespace SemanticRelease.CommitAnalyzer
{
    public class DotnetProjectParser : IProjectManager
    {
        public string ProjectPath { get; }
        private readonly Project _project;
        private string _version;

        private PathBase Path { get; }
        private DirectoryBase Directory { get; }

        public DotnetProjectParser(string projectPath, IFileSystem fileSystem)
        {
            Path = fileSystem.Path;
            Directory = fileSystem.Directory;

            var workingDir = Path.GetDirectoryName(projectPath) + "/";
            ProjectPath = FindProjPath(projectPath);
            _project = DotnetCoreBuildTools.GetCoreProject(ProjectPath);
        }

        public string GetVersion()
        {
            var props = _project.Xml.PropertyGroups.First(); // Need to make sure no other property groups exist with version in them.

            var version = props.Properties.Where(o => o.Name.Equals("Version")).FirstOrDefault();
            var packageVer = props.Properties.FirstOrDefault(o => o.Name.Equals("PackageVersion"));

            return version?.Value ?? packageVer?.Value ?? string.Empty;
        }

        public void SetVersion(string version)
        {
            _version = version;

            var props = _project.Xml.PropertyGroups.First(); // Need to make sure no other property groups exist with version in them.
            props.SetProperty("Version", _version);

            var packageVer = props.Properties.FirstOrDefault(o => o.Name.Equals("PackageVersion"));
            if (!string.IsNullOrEmpty(packageVer?.Name)) props.SetProperty(packageVer.Name, _version);

            _project.Save();
        }

        private string FindProjPath(string currentDir, int maxDepth = 3, int currentDepth = 0)
        {
            try
            {
                currentDepth++;

                if (currentDepth > maxDepth) throw new Exception();

                var project = Directory.EnumerateFiles(currentDir, "*.csproj").FirstOrDefault();

                if (string.IsNullOrEmpty(project))
                    return FindProjPath(Directory.GetParent(currentDir).FullName, maxDepth, currentDepth);
                else
                    return project;
            }
            catch (Exception)
            {
                throw new Exception("Unable to locate dotnet project path.");
            }
        }
    }
}