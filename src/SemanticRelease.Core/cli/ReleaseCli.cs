using System;
using SemanticRelease.CommitAnalyzer;
using SemanticRelease.Extensibility.Model;
using McMaster.Extensions.CommandLineUtils;
using SemanticRelease.Extensibility;
using System.IO.Abstractions;

namespace SemanticRelease.Core.CLI
{
    [Command(Description = "Create new version and prepare for release")]
    public class ReleaseCli : ToolCliBase
    {
        private readonly FileSystem _fileSystem;

        private SemanticReleaseEntry Parent { get; set; }

        [Option("-f|--fail-on-no-release",  Description = "Return non-zero exit code when no release is required.")]
        public bool ThrowOnNoOp { get; set; }

        [Option("-d|--detachedHead", Description = "Work with a detached HEAD (for example, when building on Azure pipelines)")]
        public bool DetachedHead { get; set; }

        public ReleaseCli()
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

                var preconditions = new DefaultPreConditions(repository);
                preconditions.Verify(DetachedHead);

                var commitAnalyzer = new DefaultCommitAnalyzer(repository);
                commitAnalyzer.CommitEvent += OnCommitEvent;
                var nextRelease = commitAnalyzer.CalculateNextRelease();

                var project = new DotnetProjectParser(workingDir, _fileSystem);
                project.SetVersion(nextRelease.Version);

                var releaser = new ProjectReleaser(project, repository, _fileSystem);
                releaser.PrepareForRelease();
            }
            catch (NoOpReleaseException ex)
            {
                Console.WriteLine($"There have been no releasable commits since v{ex.LastVersion}");
                return ThrowOnNoOp ? 1 : 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }

            return 0;
        }

        private void OnCommitEvent(object sender, CommitStatusEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}