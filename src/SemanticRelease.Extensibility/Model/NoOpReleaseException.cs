using System;
using System.Runtime.Serialization;

namespace SemanticRelease.Extensibility.Model
{
    [Serializable]
    public class NoOpReleaseException : Exception
    {
        public string LastVersion { get; }

        public NoOpReleaseException(string lastVersion) : base()
        {
            LastVersion = lastVersion;
        }
        public NoOpReleaseException(string lastVersion, string message) : base(message)
        {
            LastVersion = lastVersion;
        }

        public NoOpReleaseException(string lastVersion, string message, Exception inner)
        : base(message, inner)
        {
            LastVersion = lastVersion;
        }
        protected NoOpReleaseException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}