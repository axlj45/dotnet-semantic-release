using System;
using McMaster.Extensions.CommandLineUtils;
using AxlSoft.SemanticRelease.CommitAnalyzer;

namespace AxlSoft.SemanticRelease.Tool
{
    [Command(Description = "My global command line tool.")]
    class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Argument(0, Description = "Path to location where project to version resides.")]
        public string TargetProject { get; }

        [Argument(1, Description = "Name of the branch to release on.")]
        public string ReleaseBranch { get; } = "trunk";

        private int OnExecute()
        {
            var workingDir = TargetProject ?? System.Environment.CurrentDirectory;

            try
            {
                var repository = new GitRepositorySingleton();
                var releaseRepository = repository.InitializeRepository(workingDir, ReleaseBranch);

                var preconditions = new DefaultPreConditions();
                preconditions.Verify();

                var commitAnalyzer = new CommitAnalyzer.CommitAnalyzer();
                var nextRelease = commitAnalyzer.CalculateNextRelease();

                var project = new DotnetProjectWrapper(workingDir);
                project.SetVersion(nextRelease.Version);
                project.PrepareForRelease();
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