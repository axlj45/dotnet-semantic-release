using System;
using System.Linq;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using SemanticRelease.Extensibility;
using SemanticRelease.Extensibility.Model;

namespace SemanticRelease.CommitAnalyzer
{
    public class GitHubSourcePublisher : ISourcePublisher
    {
        private readonly ReleaseRepository<IRepository> _repoRef;
        private readonly IRepository repo;

        public GitHubSourcePublisher(ISourceRepositoryProvider repositoryProvider)
        {
            _repoRef = repositoryProvider.RepositoryRef as ReleaseRepository<IRepository>;
            this.repo = _repoRef.GetRepositoryReference();
        }

        public void Push()
        {
            var currentBranch = repo.Head.FriendlyName;
            var releaseBranch = _repoRef.ReleaseBranch;

            if (!string.Equals(releaseBranch, currentBranch))
                throw new NoOpReleaseException($"Current branch '{currentBranch}' does not match release branch: {releaseBranch}");

            var options = GetPushOptions();
            repo.Network.Push(repo.Head, options);
        }

        public void Push(string tagToPush)
        {
            Push();
            var options = GetPushOptions();

            var selectedTag = repo
                .Tags
                .FirstOrDefault(o => o.FriendlyName.Equals(tagToPush, StringComparison.CurrentCultureIgnoreCase))
                ?.CanonicalName;

            if (string.IsNullOrEmpty(selectedTag))
                throw new Exception($"Cannot push {tagToPush}, tag not found.");

            var origin = repo.Network.Remotes["origin"];

            repo.Network.Push(origin, selectedTag, options);
        }

        private PushOptions GetPushOptions()
        {
            var options = new PushOptions();
            options.CredentialsProvider = GetCredentialsHandler();
            return options;
        }

        private CredentialsHandler GetCredentialsHandler()
        {
            string user = Environment.GetEnvironmentVariable("GH_USER");
            string token = Environment.GetEnvironmentVariable("GH_TOKEN");
            var handler = new CredentialsHandler(
                            (url, usernameFromUrl, types) => new UsernamePasswordCredentials()
                            {
                                Username = usernameFromUrl ?? user,
                                Password = token
                            });
            return handler;
        }
    }
}