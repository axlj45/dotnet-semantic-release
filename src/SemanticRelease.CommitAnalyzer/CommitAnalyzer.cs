using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SemanticRelease.Extensibility;
using SemanticRelease.Extensibility.Model;
using LibGit2Sharp;
using SemanticVersion = SemVer.Version;

namespace SemanticRelease.CommitAnalyzer
{
    public class CommitAnalyzer : ICommitAnalyzer
    {
        private ReleaseRepository _repository;

        public CommitAnalyzer()
        {
            this._repository = new GitRepositorySingleton().GetRepository();
        }

        public Release CalculateNextRelease()
        {
            var lastRelease = GetLastRelease();

            var msg = "This is the first release.";

            Console.WriteLine($"Last Release: {lastRelease?.Version.ToString() ?? msg}");

            var commitsSinceRelease = CommitsSinceLastRelease(lastRelease).ToList();

            var releaseType = new CommitMessageParser(commitsSinceRelease).GetReleaseType();

            Console.WriteLine($"Release type: {releaseType}");

            var nextVersion = new VersionCalculator(lastRelease, releaseType).GetNextVersion();

            Console.WriteLine($"Next version: {nextVersion}");

            return new Release(nextVersion.ToString(), null);
        }

        private IEnumerable<ReleaseCommit> CommitsSinceLastRelease(Release lastRelease)
        {
            var repository = _repository.GetRepositoryReference<Repository>();

            var lastReleaseCommit = repository.Commits.FirstOrDefault(o => o.Sha.Equals(lastRelease?.Sha));
            int index = 0;

            foreach (var commit in repository.Commits)
            {
                if (commit.Sha.Equals(lastReleaseCommit?.Sha)) break;

                yield return new ReleaseCommit(index++, commit.Sha, commit.Message);
            }
        }

        private Release GetLastRelease()
        {
            var repository = _repository.GetRepositoryReference<Repository>();

            var lastRelease = repository.Tags.Where(o =>
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