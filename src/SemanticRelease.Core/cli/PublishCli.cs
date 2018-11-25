using System;
using System.IO.Abstractions;
using McMaster.Extensions.CommandLineUtils;
using SemanticRelease.CommitAnalyzer;
using SemanticRelease.Core.CLI;

namespace SemanticRelease.Core.CLI
{
    [Command(Description = "Create new version and prepare for release")]
    public class PublishCli : ToolCliBase
    {
        private readonly FileSystem _fileSystem;

        private SemanticReleaseEntry Parent { get; set; }


        public PublishCli()
        {
            _fileSystem = new FileSystem();
        }

        protected override int OnExecute(CommandLineApplication app)
        {
            var workingDir = TargetProject ?? Parent.TargetProject ?? System.Environment.CurrentDirectory;
            var releaseBranch = ReleaseBranch ?? Parent.ReleaseBranch ?? "trunk";

            try
            {
                var repository = new OnDiskGitRepository(workingDir, releaseBranch, _fileSystem);

                var publisher = new GitHubSourcePublisher(repository);
                var commitAnalyzer = new DefaultCommitAnalyzer(repository);

                var lastRelease = commitAnalyzer.GetLastRelease();

                publisher.Push(lastRelease.Version);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }

            return 0;
        }
    }
}