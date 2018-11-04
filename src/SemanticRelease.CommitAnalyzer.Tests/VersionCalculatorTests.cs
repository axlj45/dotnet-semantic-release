using SemanticRelease.Extensibility;
using SemanticRelease.Extensibility.Model;
using Xunit;

namespace SemanticRelease.CommitAnalyzer.Tests
{
    public class VersionCalculatorTests
    {
        [Fact]
        public void MajorReleaseShouldIncrementMajorVersion()
        {
            var lastRelease = new Release("1.0.0", "DEADBEEF");
            var versionCalculator = new VersionCalculator(lastRelease, ReleaseType.MAJOR);
            var nextVersion = versionCalculator.GetNextVersion();

            Assert.Equal("2.0.0", nextVersion.ToString());
        }

        [Fact]
        public void MinorReleaseShouldIncrementMinorVersion()
        {
            var lastRelease = new Release("1.0.0", "DEADBEEF");
            var versionCalculator = new VersionCalculator(lastRelease, ReleaseType.MINOR);
            var nextVersion = versionCalculator.GetNextVersion();

            Assert.Equal("1.1.0", nextVersion.ToString());
        }


        [Fact]
        public void PatchReleaseShouldIncrementPatchVersion()
        {
            var lastRelease = new Release("1.0.0", "DEADBEEF");
            var versionCalculator = new VersionCalculator(lastRelease, ReleaseType.PATCH);
            var nextVersion = versionCalculator.GetNextVersion();

            Assert.Equal("1.0.1", nextVersion.ToString());
        }

        [Fact]
        public void NoReleaseShouldThrowExeption()
        {
            var lastRelease = new Release("1.0.0", "DEADBEEF");
            var versionCalculator = new VersionCalculator(lastRelease, ReleaseType.NONE);
            Assert.Throws<NoOpReleaseException>(() => versionCalculator.GetNextVersion());
        }

        [Fact]
        public void VersionShouldDefaultToOneO()
        {
            Release lastRelease = null;
            var versionCalculator = new VersionCalculator(lastRelease, ReleaseType.MAJOR);
            var nextVersion = versionCalculator.GetNextVersion();

            Assert.Equal("1.0.0", nextVersion.ToString());
        }
    }
}