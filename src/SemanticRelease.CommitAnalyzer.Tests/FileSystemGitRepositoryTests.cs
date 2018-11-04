using System.IO;
using Xunit;

namespace SemanticRelease.CommitAnalyzer.Tests
{
    public class FileSystemGitRepositoryTests
    {
        [Fact]
        public void CannotInitializeBadFileSystemGitRepository()
        {
            Assert.Throws<FileNotFoundException>(() => new OnDiskGitRepository("trunk", "./"));
        }

        [Fact(Skip = "Not ready yet...")]
        public void CanInitializeFileSystemGitRepository()
        {
            var repository = new OnDiskGitRepository("trunk", "./");

            Assert.Equal("InMemory", repository.RepositoryPath);
            Assert.Equal("trunk", repository.ReleaseBranch);
        }
    }
}