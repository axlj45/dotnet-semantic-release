using System;
using SemanticRelease.CommitAnalyzer;
using McMaster.Extensions.CommandLineUtils;
using System.IO.Abstractions;

namespace SemanticRelease.Core.CLI
{
    [Command(Description = "Get version of target project")]
    public class CurrentVersionCli : ToolCliBase
    {
        private readonly FileSystem _fileSystem;

        private SemanticReleaseEntry Parent { get; set; }

        public CurrentVersionCli()
        {
            _fileSystem = new FileSystem();
        }

        protected override int OnExecute(CommandLineApplication app)
        {
            var targetProject = TargetProject ?? Parent.TargetProject;

            var project = new DotnetProjectParser(targetProject ?? System.Environment.CurrentDirectory, _fileSystem);

            Console.WriteLine(project.GetVersion());

            return 0;
        }
    }
}