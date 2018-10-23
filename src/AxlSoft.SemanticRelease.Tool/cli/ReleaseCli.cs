using System;
using AxlSoft.SemanticRelease.CommitAnalyzer;
using McMaster.Extensions.CommandLineUtils;

namespace AxlSoft.SemanticRelease.Tool.cli
{
    [Command(Description = "Create new version and prepare for release")]
    public class ReleaseCli : ToolCliBase
    {
        private SemanticReleaseEntry Parent { get; set; }

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }

            return 0;
        }
    }
}