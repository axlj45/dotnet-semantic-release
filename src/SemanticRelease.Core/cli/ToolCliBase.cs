using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace SemanticRelease.Core.CLI
{
    [HelpOption("--help")]
    public class ToolCliBase
    {
        [Option("-p|--project-path <path>", Description = "The directory that contains the csproj project to release.")]
        [FileOrDirectoryExists]
        public string TargetProject { get; set; }

        [Option("-b|--release-branch <branch>", Description = "Branch that is permitted to perform a release.  Default is 'trunk'")]
        public string ReleaseBranch { get; set; }

        protected virtual int OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return 0;
        }
    }
}