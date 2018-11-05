using Xunit;
using System.IO.Abstractions.TestingHelpers;

namespace SemanticRelease.CommitAnalyzer.Tests
{
    public class FileSystemGitRepositoryTests
    {
        [Fact]
        public void CannotInitializeBadFileSystemGitRepository()
        {
            var emptyFileSystem = new MockFileSystem();
            Assert.Throws<System.IO.FileNotFoundException>(() => new OnDiskGitRepository("trunk", "./", emptyFileSystem));
        }

        [Fact(Skip = "Not ready yet...")]
        public void CanInitializeFileSystemGitRepository()
        {
            var repository = new OnDiskGitRepository("trunk", "./", new MockFileSystem());

            Assert.Equal("InMemory", repository.RepositoryPath);
            Assert.Equal("trunk", repository.ReleaseBranch);
        }
    }
}