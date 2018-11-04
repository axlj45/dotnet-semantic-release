using System;
using System.Collections.Generic;
using System.Linq;
using SemanticRelease.Extensibility;
using SemanticRelease.Extensibility.Model;
using LibGit2Sharp;
using SemanticVersion = SemVer.Version;

namespace SemanticRelease.CommitAnalyzer
{
    public class DefaultCommitAnalyzer : ICommitAnalyzer
    {
        private ISourceRepositoryProvider _repository;
        private readonly IRepository _repoReference;
        private readonly VersionCalculator _versionCalculator;

        public event EventHandler<CommitStatusEventArgs> CommitEvent;

        public DefaultCommitAnalyzer(ISourceRepositoryProvider repositoryProvider)
        {
            this._repository = repositoryProvider;
            this._repoReference = repositoryProvider.RepositoryRef as IRepository;
            this._versionCalculator = new VersionCalculator();
        }

        public Release CalculateNextRelease()
        {
            var lastRelease = GetLastRelease();

            var msg = "This is the first release.";

            SendEvent($"Last Release: {lastRelease?.Version.ToString() ?? msg}");

            var commitsSinceRelease = CommitsSinceLastRelease(lastRelease).ToList();

            var releaseType = new CommitMessageParser(commitsSinceRelease).GetReleaseType();

            SendEvent($"Release type: {releaseType}");

            var nextVersion = _versionCalculator.GetNextVersion(lastRelease, releaseType);

            SendEvent($"Next version: {nextVersion}");

            return new Release(nextVersion.ToString(), null);
        }

        private void SendEvent(string message)
        {
            CommitEvent?.Invoke(this, new CommitStatusEventArgs(message));
        }

        private IEnumerable<ReleaseCommit> CommitsSinceLastRelease(Release lastRelease)
        {
            var lastReleaseCommit = _repoReference.Commits.FirstOrDefault(o => o.Sha.Equals(lastRelease?.Sha));
            int index = 0;

            foreach (var commit in _repoReference.Commits)
            {
                if (commit.Sha.Equals(lastReleaseCommit?.Sha)) break;

                yield return new ReleaseCommit(index++, commit.Sha, commit.Message);
            }
        }

        private Release GetLastRelease()
        {
            var lastRelease = _repoReference.Tags.Where(o =>
             {
                 try
                 {
                     var version = new SemanticVersion(o.FriendlyName);
                     return string.IsNullOrEmpty(version.PreRelease);
                 }
                 catch
                 {
                     return false;
                 }
             })
            .Select(o => new { Commit = o, Version = new SemanticVersion(o.FriendlyName) })
            .OrderByDescending(o => o.Version)
            .Select(o => new Release(o.Version.ToString(), o.Commit.PeeledTarget.Sha))
            .FirstOrDefault();

            return lastRelease;
        }
    }
}