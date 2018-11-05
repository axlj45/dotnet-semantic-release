using System;
using System.IO.Abstractions;
using System.Linq;
using LibGit2Sharp;
using SemanticRelease.Extensibility;
using SemanticRelease.Extensibility.Model;

namespace SemanticRelease.CommitAnalyzer
{
    public class OnDiskGitRepository : ISourceRepositoryProvider<IRepository>
    {
        private readonly ReleaseRepository<IRepository> _repository;

        public IRepository RepositoryRef => _repository?.GetRepositoryReference();

        public string ReleaseBranch => _repository?.ReleaseBranch;

        public string RepositoryPath => _repository?.RepositoryPath;

        object ISourceRepositoryProvider.RepositoryRef => _repository;

        private DirectoryBase Directory { get; }

        public OnDiskGitRepository(string repoPath, string releaseBranch, IFileSystem fileSystem)
        {
            Directory = fileSystem.Directory;

            var gitPath = FindGitPath(repoPath);
            var repoRef = new Repository(gitPath);

            _repository = new ReleaseRepository<IRepository>(gitPath, releaseBranch, repoRef);
        }

        private string FindGitPath(string currentPath, int maxDepth = 3, int currentDepth = 0)
        {
            try
            {
                currentDepth++;

                if (currentDepth > maxDepth) throw new Exception();

                var dir = Directory.EnumerateDirectories(currentPath, ".git").FirstOrDefault();

                if (string.IsNullOrEmpty(dir))
                    return FindGitPath(Directory.GetParent(currentPath).FullName, maxDepth, currentDepth);
                else
                    return dir;
            }
            catch (Exception ex)
            {
                throw new System.IO.FileNotFoundException("Unable to locate git repository for project.", ex);
            }
        }
    }
}