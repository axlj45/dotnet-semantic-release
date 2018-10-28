using System;
using SemanticRelease.Extensibility;
using SemanticRelease.Extensibility.Model;
using LibGit2Sharp;

namespace SemanticRelease.CommitAnalyzer
{
    public class DefaultPreConditions : IPreConditionVerifier
    {
        private ReleaseRepository _repository;

        public DefaultPreConditions()
        {
            this._repository = new GitRepositorySingleton().GetRepository();
        }

        public void Verify()
        {
            var repo = _repository.GetRepositoryReference<Repository>();

            if (!repo.Head.FriendlyName.Equals(_repository.ReleaseBranch))
            {
                throw new Exception($"Wrong Branch: {repo.Head.FriendlyName} must be {_repository.ReleaseBranch} to deploy");
            }

            if (!repo.Head.IsTracking)
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