namespace SemanticRelease.Extensibility.Model
{
    public abstract class AbstractReleaseRepository
    {
        public virtual string ReleaseBranch { get; }
        public virtual string RepositoryPath { get; }
        protected virtual object RepositoryRef { get; }

        protected AbstractReleaseRepository(string repositoryPath, string releaseBranchName, object repositoryReference)
        {
            this.RepositoryPath = repositoryPath;
            this.ReleaseBranch = releaseBranchName;
            this.RepositoryRef = repositoryReference;
        }
    }
    public class ReleaseRepository<T> : AbstractReleaseRepository
    {

        public ReleaseRepository(string repositoryPath, string releaseBranchName, T repositoryReference)
        : base(repositoryPath, releaseBranchName, repositoryReference) { }

        public T GetRepositoryReference()
        {
            return (T)RepositoryRef;
        }
    }
}