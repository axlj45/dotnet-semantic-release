using SemanticRelease.Extensibility.Model;

namespace SemanticRelease.Extensibility
{
    public interface ISourceRepositoryProvider
    {
        string ReleaseBranch { get; }
        string RepositoryPath { get; }

        object RepositoryRef { get; }
    }

    public interface ISourceRepositoryProvider<T> : ISourceRepositoryProvider
    {
        new T RepositoryRef { get; }
    }
}