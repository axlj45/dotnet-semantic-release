using System;
using System.IO;
using SemanticRelease.Extensibility.Model;
using LibGit2Sharp;

namespace SemanticRelease.CommitAnalyzer
{
    public class ProjectReleaser
    {
        private readonly ReleaseRepository _gitRepo;
        private readonly DotnetProjectWrapper _project;

        public ProjectReleaser(DotnetProjectWrapper project)
        {
            _project = project;
            _gitRepo = new GitRepositorySingleton().GetRepository();
        }
        public void PrepareForRelease()
        {
            var repo = _gitRepo.GetRepositoryReference<Repository>();

            int workDirLength = Directory.GetParent(_gitRepo.RepositoryPath).FullName.Length;
            string workingPath = Path.GetFullPath(_project.ProjectPath).Substring(workDirLength + 1);

            repo.Index.Add(workingPath);
            var signature = new Signature("jenkins", "jenkins", DateTimeOffset.UtcNow);
            var vCommit = repo.Commit($"chore(release): Releasing {_project.GetVersion()}", signature, signature);
            var vTag = repo.ApplyTag(_project.GetVersion());
        }
    }
}