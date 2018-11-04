using System;
using System.IO;
using SemanticRelease.Extensibility.Model;
using LibGit2Sharp;
using SemanticRelease.Extensibility;

namespace SemanticRelease.CommitAnalyzer
{
    public class ProjectReleaser : IProjectReleaseStrategy
    {
        private readonly ISourceRepositoryProvider<IRepository> _gitRepo;
        private readonly DotnetProjectWrapper _project;

        public ProjectReleaser(DotnetProjectWrapper project, ISourceRepositoryProvider repo)
        {
            _project = project;
            _gitRepo = (ISourceRepositoryProvider<IRepository>)repo;
        }
        public void PrepareForRelease()
        {
            var repo = _gitRepo.RepositoryRef;

            int workDirLength = Directory.GetParent(_gitRepo.RepositoryPath).FullName.Length;
            string workingPath = Path.GetFullPath(_project.ProjectPath).Substring(workDirLength + 1);

            repo.Index.Add(workingPath);
            var signature = new Signature("jenkins", "jenkins", DateTimeOffset.UtcNow);
            var vCommit = repo.Commit($"chore(release): Releasing {_project.GetVersion()}", signature, signature);
            var vTag = repo.ApplyTag(_project.GetVersion());
        }
    }
}