namespace SemanticRelease.Extensibility.Model
{
    public class SemanticReleaseVersion
    {
        public string Version { get; }

        public SemanticReleaseVersion(string version)
        {
            this.Version = version;
        }

        public override string ToString() => this.Version;
    }
}