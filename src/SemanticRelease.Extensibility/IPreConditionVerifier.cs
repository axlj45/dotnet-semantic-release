namespace SemanticRelease.Extensibility
{
    public interface IPreConditionVerifier
    {
        void Verify(bool detachedHead);
    }
}