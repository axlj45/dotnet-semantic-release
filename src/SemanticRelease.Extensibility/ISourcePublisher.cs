namespace SemanticRelease.Extensibility
{
    public interface ISourcePublisher
    {
        void Push();
        void Push(string tagToPush);
    }
}