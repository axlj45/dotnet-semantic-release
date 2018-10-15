using System;
using System.IO;
using System.Linq;
using AxlSoft.SemanticRelease.Extensibility.Model;
using LibGit2Sharp;

namespace AxlSoft.SemanticRelease.CommitAnalyzer
{
    public class GitRepositorySingleton
    {
        private static ReleaseRepository _repository;

        public ReleaseRepository InitializeRepository(string repoPath, string releaseBranch)
        {
            if (_repository != null) throw new Exception("Cannot create duplicate repositories");

            var gitPath = FindGitPath(repoPath);
            var repoRef = new Repository(gitPath);

            _repository = new ReleaseRepository(gitPath, releaseBranch, repoRef);
            return _repository;
        }

        public ReleaseRepository InitializeRepository(string releaseBranch)
        {
            if (_repository != null) throw new Exception("Cannot create duplicate repositories.");

            var repoRef = new Repository();

            _repository = new ReleaseRepository("InMemory", releaseBranch, repoRef);
            return _repository;
        }

        public ReleaseRepository GetRepository()
        {
            if (_repository == null) throw new Exception("Source repository not initialized");

            return _repository;
        }

        private string FindGitPath(string currentPath, int maxDepth = 3, int currentDepth = 0)
        {
            try
            {
                currentDepth++;

                if (currentDepth > maxDepth) throw new Exception();

                var dir = Directory.EnumerateDirectories(currentPath, ".git").FirstOrDefault();

                if (string.IsNullOrEmpty(dir))
                    return FindGitPath(Directory.GetParent(currentPath).FullName, maxDepth, currentDepth);
                else
                    return dir;
            }
            catch (Exception)
            {
                throw new Exception("Unable to locate git repository for project.");
            }
        }
    }
}