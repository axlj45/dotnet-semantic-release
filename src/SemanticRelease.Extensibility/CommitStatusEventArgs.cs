using System;

namespace SemanticRelease.Extensibility
{
    public class CommitStatusEventArgs : EventArgs
    {
        public string Message { get; }

        public CommitStatusEventArgs(string message)
        {
            this.Message = message;
        }
    }
}