using System;
using System.IO;
using AxlSoft.SemanticRelease.Extensibility.Model;
using LibGit2Sharp;

namespace AxlSoft.SemanticRelease.CommitAnalyzer
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

            repo.Index.Add(_project.ProjectPath.ReplaceFirst(Directory.GetParent(_gitRepo.RepositoryPath).FullName + "/", ""));
            var signature = new Signature("jenkins", "jenkins", DateTimeOffset.UtcNow);
            var vCommit = repo.Commit($"chore(release): Releasing {_project.GetVersion()}", signature, signature);
            var vTag = repo.ApplyTag(_project.GetVersion());
        }
    }
}