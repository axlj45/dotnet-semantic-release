using System;
using System.IO;
using System.Linq;
using Microsoft.Build.Evaluation;

namespace SemanticRelease.CommitAnalyzer
{
    public class DotnetProjectWrapper
    {
        public string ProjectPath { get; }
        private readonly Project _project;
        private string _version;

        public DotnetProjectWrapper(string projectPath)
        {
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