using SemanticRelease.Extensibility.Model;

namespace SemanticRelease.Extensibility
{
    public interface IVersionCalculator
    {
        SemanticReleaseVersion GetNextVersion(Release lastRelease, ReleaseType releaseType);
    }
}