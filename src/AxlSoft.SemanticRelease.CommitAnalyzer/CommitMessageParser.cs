using System.Collections.Generic;
using System.Text.RegularExpressions;
using AxlSoft.SemanticRelease.Extensibility;
using AxlSoft.SemanticRelease.Extensibility.Model;

namespace AxlSoft.SemanticRelease.CommitAnalyzer
{
    internal class CommitMessageParser
    {
        private readonly IEnumerable<ReleaseCommit> _commitsSinceRelease;

        public CommitMessageParser(IEnumerable<ReleaseCommit> commitsSinceRelease)
        {
            _commitsSinceRelease = commitsSinceRelease;

        }

        public ReleaseType GetReleaseType()
        {
            var releaseType = ReleaseType.NONE;

            var multiLineIgnoreCase = RegexOptions.IgnoreCase | RegexOptions.Singleline;

            var majorRelease = new Regex("(BREAKING)", RegexOptions.Singleline);
            var minorRelease = new Regex(@"(feat:|feature:|feat\(.*\))", multiLineIgnoreCase);
            var patchRelease = new Regex(@"(fix|perf|security)(\(.*\))?:", multiLineIgnoreCase);

            foreach (var commit in _commitsSinceRelease)
            {
                if (majorRelease.IsMatch(commit.Message))
                {
                    releaseType = ReleaseType.MAJOR;
                    break;
                }

                if (minorRelease.IsMatch(commit.Message) || releaseType == ReleaseType.MINOR)
                {
                    releaseType = ReleaseType.MINOR;
                    continue;
                }

                if (patchRelease.IsMatch(commit.Message))
                {
                    releaseType = ReleaseType.PATCH;
                }
            }

            return releaseType;
        }
    }
}