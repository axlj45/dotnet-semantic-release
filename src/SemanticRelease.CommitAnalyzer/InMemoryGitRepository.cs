using System;
using LibGit2Sharp;
using SemanticRelease.Extensibility;
using SemanticRelease.Extensibility.Model;

namespace SemanticRelease.CommitAnalyzer
{
    public class InMemoryGitRepository : ISourceRepositoryProvider<IRepository>
    {
        private readonly ReleaseRepository<IRepository> _repository;

        public string ReleaseBranch => _repository.ReleaseBranch;

        public string RepositoryPath => _repository.RepositoryPath;

        public IRepository RepositoryRef => (IRepository)_repository.GetRepositoryReference();

        object ISourceRepositoryProvider.RepositoryRef => _repository.GetRepositoryReference();

        public InMemoryGitRepository(string releaseBranch)
        {
            var repoRef = new Repository();

            _repository = new ReleaseRepository<IRepository>("InMemory", releaseBranch, repoRef);
        }
    }
}