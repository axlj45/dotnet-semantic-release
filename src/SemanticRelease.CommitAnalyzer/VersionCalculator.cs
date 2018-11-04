using System;
using SemanticRelease.Extensibility;
using SemanticRelease.Extensibility.Model;
using SemanticVersion = SemVer.Version;

namespace SemanticRelease.CommitAnalyzer
{
    internal class VersionCalculator : IVersionCalculator
    {

        public SemanticReleaseVersion GetNextVersion(Release lastRelease, ReleaseType releaseType)
        {
            var lastVersion = lastRelease?.Version;

            if (lastVersion == null) return new SemanticReleaseVersion("1.0.0");

            var testVersion = new SemanticVersion(lastVersion);

            int nextMajor = testVersion.Major;
            int nextMinor = testVersion.Minor;
            int nextPatch = testVersion.Patch;

            switch (releaseType)
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
                    throw new NoOpReleaseException(lastVersion);
            }

            var newVersion = new SemanticVersion($"{nextMajor}.{nextMinor}.{nextPatch}");

            return new SemanticReleaseVersion(newVersion.ToString());
        }
    }
}