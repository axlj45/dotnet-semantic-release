using System;
using System.Reflection;
using AxlSoft.SemanticRelease.CommitAnalyzer;
using McMaster.Extensions.CommandLineUtils;

namespace AxlSoft.SemanticRelease.Tool.cli
{
    [Command("semantic-release", Description = "Semantic Release")]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand("release", typeof(ReleaseCli))]
    [Subcommand("project-version", typeof(CurrentVersionCli))]
    public class SemanticReleaseEntry : ToolCliBase
    {
        private static string GetVersion()
            => typeof(SemanticReleaseEntry).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        protected override int OnExecute(CommandLineApplication app)
        {
            base.OnExecute(app);
            return 0;
        }
    }
}