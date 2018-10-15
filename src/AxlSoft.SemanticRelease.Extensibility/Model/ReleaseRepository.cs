namespace AxlSoft.SemanticRelease.Extensibility.Model
{
    public class ReleaseRepository
    {
        public string ReleaseBranch { get; }
        public string RepositoryPath { get; }
        private object _repositoryReference;

        public ReleaseRepository(string repositoryPath, string releaseBranchName, object repositoryReference)
        {
            this.ReleaseBranch = releaseBranchName;
            this.RepositoryPath = repositoryPath;
            _repositoryReference = repositoryReference;
        }

        public T GetRepositoryReference<T>()
        {
            return (T)_repositoryReference;
        }
    }
}