using McMaster.Extensions.CommandLineUtils;
using AxlSoft.SemanticRelease.Tool.cli;

namespace AxlSoft.SemanticRelease.Tool
{
    [Command(Description = "Semantic Release")]
    class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<SemanticReleaseEntry>(args);
    }
}