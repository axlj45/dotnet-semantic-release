using System;
using SemanticRelease.CommitAnalyzer;
using SemanticRelease.Extensibility.Model;
using McMaster.Extensions.CommandLineUtils;

namespace SemanticRelease.Tool.CLI
{
    [Command(Description = "Create new version and prepare for release")]
    public class ReleaseCli : ToolCliBase
    {
        private SemanticReleaseEntry Parent { get; set; }

        [Option("-f|--fail-on-no-release <branch>", Description = "Return non-zero exit code when no release is required.")]
        public bool ThrowOnNoOp { get; set; }

        protected override int OnExecute(CommandLineApplication app)
        {
            var workingDir = TargetProject ?? Parent.TargetProject ?? System.Environment.CurrentDirectory;
            var releaseBranch = ReleaseBranch ?? Parent.ReleaseBranch ?? "trunk";

            try
            {
                var repository = new GitRepositorySingleton();
                var releaseRepository = repository.InitializeRepository(workingDir, releaseBranch);

                var preconditions = new DefaultPreConditions();
                preconditions.Verify();

                var commitAnalyzer = new CommitAnalyzer.CommitAnalyzer();
                var nextRelease = commitAnalyzer.CalculateNextRelease();

                var project = new DotnetProjectWrapper(workingDir);
                project.SetVersion(nextRelease.Version);

                var releaser = new ProjectReleaser(project);
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
    }
}