namespace SemanticRelease.Extensibility
{
    public interface IProjectManager
    {
        string ProjectPath { get; }
        string GetVersion();
        void SetVersion(string version);
    }
}