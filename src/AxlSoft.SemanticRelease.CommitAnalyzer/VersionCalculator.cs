using System;
using AxlSoft.SemanticRelease.Extensibility;
using AxlSoft.SemanticRelease.Extensibility.Model;
using SemanticVersion = SemVer.Version;

namespace AxlSoft.SemanticRelease.CommitAnalyzer
{
    internal class VersionCalculator
    {
        private readonly Release _lastRelease;
        private readonly ReleaseType _releaseType;

        public VersionCalculator(Release lastRelease, ReleaseType releaseType)
        {
            _releaseType = releaseType;
            _lastRelease = lastRelease;
        }

        public SemanticVersion GetNextVersion()
        {
            var lastVersion = _lastRelease?.Version;

            if (lastVersion == null) return new SemanticVersion("1.0.0");

            var testVersion = new SemanticVersion(lastVersion);

            int nextMajor = testVersion.Major;
            int nextMinor = testVersion.Minor;
            int nextPatch = testVersion.Patch;

            switch (_releaseType)
            {
                case ReleaseType.MAJOR:
                    nextMajor += 1;
                    nextMinor = 0;
                    nextPatch = 0;
                    break;

                case ReleaseType.MINOR:
                    nextMinor += 1;
                    nextPatch = 0;
                    break;

                case ReleaseType.PATCH:
                    nextPatch += 1;
                    break;

                default:
                    throw new Exception($"There have been no releasable commits since v{lastVersion}.");
            }

            return new SemanticVersion($"{nextMajor}.{nextMinor}.{nextPatch}");
        }
    }
}