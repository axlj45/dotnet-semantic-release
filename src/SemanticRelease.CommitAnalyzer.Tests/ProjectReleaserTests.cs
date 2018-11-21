using System.IO.Abstractions.TestingHelpers;
using LibGit2Sharp;
using Moq;
using SemanticRelease.Extensibility;
using Xunit;

namespace SemanticRelease.CommitAnalyzer.Tests
{
    public class ProjectReleaserTests
    {
        private MockFileSystem _fileSystem { get; }

        public ProjectReleaserTests()
        {
            _fileSystem = new MockFileSystem();
            _fileSystem.AddDirectory("/home/projects/semantic_release/src");
        }

        [Fact]
        public void ProjectCanRelease()
        {
            string repositoryPath = "/home/projects/semantic_release";
            string projectPath = "/home/projects/semantic_release/src";

            var projectManager = new Mock<IProjectManager>();
            projectManager.SetupGet(o => o.ProjectPath).Returns(projectPath);

            var repo = new Mock<ISourceRepositoryProvider<IRepository>>();
            repo.SetupGet(o => o.RepositoryPath).Returns(repositoryPath);

            var repository = new Repository();

            repo.SetupGet(o => o.RepositoryRef).Returns(repository);

            var releaser = new ProjectReleaser(projectManager.Object, repo.Object, _fileSystem);

            // releaser.PrepareForRelease();
        }
    }
}