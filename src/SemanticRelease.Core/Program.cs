using McMaster.Extensions.CommandLineUtils;
using SemanticRelease.Tool.CLI;

namespace SemanticRelease.Tool
{
    [Command(Description = "Semantic Release")]
    class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<SemanticReleaseEntry>(args);
    }
}