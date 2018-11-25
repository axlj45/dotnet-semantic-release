using System;
using System.Reflection;
using SemanticRelease.CommitAnalyzer;
using McMaster.Extensions.CommandLineUtils;

namespace SemanticRelease.Core.CLI
{
    [Command("semantic-release", Description = "Semantic Release")]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand("release", typeof(ReleaseCli))]
    [Subcommand("project-version", typeof(CurrentVersionCli))]
    [Subcommand("publish", typeof(PublishCli))]
    public class SemanticReleaseEntry : ToolCliBase
    {
        private static string GetVersion()
            => Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        protected override int OnExecute(CommandLineApplication app)
        {
            base.OnExecute(app);
            return 0;
        }
    }
}