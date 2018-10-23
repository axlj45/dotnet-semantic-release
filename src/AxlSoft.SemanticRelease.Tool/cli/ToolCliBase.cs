using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace AxlSoft.SemanticRelease.Tool.cli
{
    [HelpOption("--help")]
    public class ToolCliBase
    {
        [Option("-p|--project-path <path>")]
        [FileOrDirectoryExists]
        public string TargetProject { get; set; }

        [Option("-b|--release-branch <branch>")]
        public string ReleaseBranch { get; set; }

        protected virtual int OnExecute(CommandLineApplication app)
        {
            Console.WriteLine($"BASE Project: {TargetProject}, Branch:  {ReleaseBranch}");
            app.ShowHelp();
            return 0;
        }
    }
}