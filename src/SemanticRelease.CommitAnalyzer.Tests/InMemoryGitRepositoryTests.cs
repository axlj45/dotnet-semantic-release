using System;
using Xunit;

namespace SemanticRelease.CommitAnalyzer.Tests
{
    public class InMemoryGitRepositoryTests
    {
        [Fact]
        public void CanInitializeInMemoryGitRepository()
        {
            var repository = new InMemoryGitRepository("trunk");

            Assert.Equal("InMemory", repository.RepositoryPath);
            Assert.Equal("trunk", repository.ReleaseBranch);
            Assert.NotNull(repository.RepositoryRef);
        }
    }
}