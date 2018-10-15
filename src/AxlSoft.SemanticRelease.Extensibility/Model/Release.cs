using System.Collections.Generic;

namespace AxlSoft.SemanticRelease.Extensibility.Model
{
    public class Release
    {
        public Release(string version, string sha) : this(version, sha, null)
        { }

        public Release(string version, string sha, IEnumerable<ReleaseCommit> commitsSinceLastRelease)
        {
            this.Version = version;
            this.Sha = sha;
            this.CommitsSinceLastRelease = commitsSinceLastRelease;
        }

        public string Version { get; private set; }
        public string Sha { get; private set; }
        public IEnumerable<ReleaseCommit> CommitsSinceLastRelease { get; private set; }
    }
}