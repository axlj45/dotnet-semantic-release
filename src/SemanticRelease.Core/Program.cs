using McMaster.Extensions.CommandLineUtils;
using SemanticRelease.Core.CLI;

namespace SemanticRelease.Core
{
    [Command(Description = "Semantic Release")]
    public class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<SemanticReleaseEntry>(args);
    }
}