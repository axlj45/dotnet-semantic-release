using System;
using SemanticRelease.Extensibility;
using LibGit2Sharp;
using SemanticRelease.Extensibility.Model;

namespace SemanticRelease.CommitAnalyzer
{
    public class DefaultPreConditions : IPreConditionVerifier
    {
        private readonly ISourceRepositoryProvider _repository;

        public DefaultPreConditions(ISourceRepositoryProvider repositoryProvider)
        {
            this._repository = repositoryProvider;
        }

        public void Verify(bool detachedHead)
        {
            var repoRef = _repository.RepositoryRef as ReleaseRepository<IRepository>;

            var repo = repoRef.GetRepositoryReference();

            if (!detachedHead && !repo.Head.FriendlyName.Equals(_repository.ReleaseBranch))
            {
                throw new Exception($"Wrong Branch: {repo.Head.FriendlyName} must be {_repository.ReleaseBranch} to deploy");
            }

            if (!detachedHead && !repo.Head.IsTracking)
            {
                throw new Exception("No remote found for current branch.");
            }

            if (!repo.Head.IsCurrentRepositoryHead)
            {
                throw new Exception("Local repository is not in sync with remote repository.");
            }
        }
    }
}