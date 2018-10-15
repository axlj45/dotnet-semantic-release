namespace AxlSoft.SemanticRelease.Extensibility.Model
{
    public class ReleaseCommit
    {
        public ReleaseCommit(int index, string sha, string message)
        {
            this.Sha = sha;
            this.Index = index;
            this.Message = message;
        }

        public int Index { get; }
        public string Sha { get; }
        public string Message { get; }
    }
}