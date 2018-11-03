using System;
using System.Collections.Generic;
using SemanticRelease.Extensibility;
using SemanticRelease.Extensibility.Model;
using Xunit;

namespace SemanticRelease.CommitAnalyzer.Tests
{
    public class CommitMessageParserTests
    {
        [Fact]
        public void NonVersionableChangesYieldsNoRelease()
        {
            var commits = new List<ReleaseCommit>();
            commits.Add(GetNonVersionableChange());

            var parser = new CommitMessageParser(commits);
            var result = parser.GetReleaseType();

            Assert.Equal(ReleaseType.NONE, result);
        }

        [Fact]
        public void FixYieldsPatchRelease()
        {
            var commits = new List<ReleaseCommit>();
            commits.Add(GetPatchCommit());

            var parser = new CommitMessageParser(commits);
            var result = parser.GetReleaseType();

            Assert.Equal(ReleaseType.PATCH, result);
        }

        [Fact]
        public void PeformanceYieldsPatchRelease()
        {
            var commits = new List<ReleaseCommit>();
            commits.Add(GetCommit("perf"));

            var parser = new CommitMessageParser(commits);
            var result = parser.GetReleaseType();

            Assert.Equal(ReleaseType.PATCH, result);
        }

        [Fact]
        public void SecurityYieldsPatchRelease()
        {
            var commits = new List<ReleaseCommit>();
            commits.Add(GetCommit("security"));

            var parser = new CommitMessageParser(commits);
            var result = parser.GetReleaseType();

            Assert.Equal(ReleaseType.PATCH, result);
        }

        [Fact]
        public void FeatureYieldsMinorRelease()
        {
            var commits = new List<ReleaseCommit>();
            commits.Add(GetFeatureCommit());

            var parser = new CommitMessageParser(commits);
            var result = parser.GetReleaseType();

            Assert.Equal(ReleaseType.MINOR, result);
        }

        [Fact]
        public void BreakingChangeYieldsMajorRelease()
        {
            var commits = new List<ReleaseCommit>();
            commits.Add(GetBreakingPatchCommit());

            var parser = new CommitMessageParser(commits);
            var result = parser.GetReleaseType();

            Assert.Equal(ReleaseType.MAJOR, result);
        }

        [Fact]
        public void MinorReleaseCommitTakesPrecedenceOverPatch()
        {
            var commits = new List<ReleaseCommit>();
            commits.Add(GetFeatureCommit());
            commits.Add(GetPatchCommit());

            var parser = new CommitMessageParser(commits);
            var result = parser.GetReleaseType();

            Assert.Equal(ReleaseType.MINOR, result);
        }

        [Fact]
        public void MajorReleaseCommitTakesPrecedenceOverMinor()
        {
            var commits = new List<ReleaseCommit>();
            commits.Add(GetFeatureCommit());
            commits.Add(GetBreakingFeatureCommit());

            var parser = new CommitMessageParser(commits);
            var result = parser.GetReleaseType();

            Assert.Equal(ReleaseType.MAJOR, result);
        }



        private ReleaseCommit GetPatchCommit()
        {
            return GetCommit("fix");
        }

        private ReleaseCommit GetFeatureCommit()
        {
            return GetCommit("feat");
        }

        private ReleaseCommit GetBreakingFeatureCommit()
        {
            return GetCommit("feat", true);
        }

        private ReleaseCommit GetBreakingPatchCommit()
        {
            return GetCommit("fix", true);
        }

        private ReleaseCommit GetNonVersionableChange()
        {
            return GetCommit("chore");
        }

        private ReleaseCommit GetNonVersionableBreakingChange()
        {
            return GetCommit("chore", true);
        }

        private ReleaseCommit GetCommit(string commitType, bool isBreaking = false, string msg = "Standard message")
        {
            string commitMsg = $"{commitType}: ${msg}";

            if (isBreaking)
            {
                commitMsg += Environment.NewLine;
                commitMsg += "BREAKING: A breaking change occurred.";
            }

            return new ReleaseCommit(0, "DEADBEEF", commitMsg);
        }
    }
}