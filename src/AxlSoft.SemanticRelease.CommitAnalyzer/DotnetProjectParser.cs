using System;
using System.IO;
using System.Linq;
using AxlSoft.SemanticRelease.Extensibility.Model;
using LibGit2Sharp;
using Microsoft.Build.Evaluation;

namespace AxlSoft.SemanticRelease.CommitAnalyzer
{
    public class DotnetProjectWrapper
    {
        private readonly string _projPath;
        private readonly Project _project;
        private readonly ReleaseRepository _gitRepo;
        private string _version;

        public DotnetProjectWrapper(string projectPath)
        {
            _projPath = FindProjPath(projectPath);
            _project = DotnetCoreBuildTools.GetCoreProject(_projPath);
            _gitRepo = new GitRepositorySingleton().GetRepository();
        }

        public void SetVersion(string version)
        {
            _version = version;

            var props = _project.Xml.PropertyGroups.First(); // Need to make sure no other property groups exist with versioi in them.
            props.SetProperty("Version", _version);

            var packageVer = props.Properties.FirstOrDefault(o => o.Name.Equals("PackageVersion"));
            if (!string.IsNullOrEmpty(packageVer?.Name)) props.SetProperty(packageVer.Name, _version);

            _project.Save();
        }

        public void PrepareForRelease()
        {
            var repo = _gitRepo.GetRepositoryReference<Repository>();

            repo.Index.Add(_projPath.ReplaceFirst(Directory.GetParent(_gitRepo.RepositoryPath).FullName + "/", ""));
            var signature = new Signature("jenkins", "jenkins", DateTimeOffset.UtcNow);
            var vCommit = repo.Commit($"chore(release): Releasing {_version}", signature, signature);
            var vTag = repo.ApplyTag(_version);
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