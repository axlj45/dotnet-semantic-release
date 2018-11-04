using SemanticRelease.Extensibility;
using SemanticRelease.Extensibility.Model;
using Xunit;

namespace SemanticRelease.CommitAnalyzer.Tests
{
    public class VersionCalculatorTests
    {
        IVersionCalculator systemUnderTest = new VersionCalculator();

        [Fact]
        public void MajorReleaseShouldIncrementMajorVersion()
        {
            var lastRelease = new Release("1.0.0", "DEADBEEF");

            var nextVersion = systemUnderTest.GetNextVersion(lastRelease, ReleaseType.MAJOR);

            Assert.Equal("2.0.0", nextVersion.ToString());
        }

        [Fact]
        public void MinorReleaseShouldIncrementMinorVersion()
        {
            var lastRelease = new Release("1.0.0", "DEADBEEF");

            var nextVersion = systemUnderTest.GetNextVersion(lastRelease, ReleaseType.MINOR);

            Assert.Equal("1.1.0", nextVersion.ToString());
        }


        [Fact]
        public void PatchReleaseShouldIncrementPatchVersion()
        {
            var lastRelease = new Release("1.0.0", "DEADBEEF");

            var nextVersion = systemUnderTest.GetNextVersion(lastRelease, ReleaseType.PATCH);

            Assert.Equal("1.0.1", nextVersion.ToString());
        }

        [Fact]
        public void NoReleaseShouldThrowExeption()
        {
            var lastRelease = new Release("1.0.0", "DEADBEEF");

            Assert.Throws<NoOpReleaseException>(() => systemUnderTest.GetNextVersion(lastRelease, ReleaseType.NONE));
        }

        [Fact]
        public void VersionShouldDefaultToOneO()
        {
            Release lastRelease = null;

            var nextVersion = systemUnderTest.GetNextVersion(lastRelease, ReleaseType.MAJOR);

            Assert.Equal("1.0.0", nextVersion.ToString());
        }
    }
}