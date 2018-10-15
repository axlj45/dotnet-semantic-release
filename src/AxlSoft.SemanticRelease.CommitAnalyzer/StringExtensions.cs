namespace AxlSoft.SemanticRelease.CommitAnalyzer
{
    public static class StringExtensionMethods
    {
        public static string ReplaceFirst(this string haystack, string needle, string replace)
        {
            int pos = haystack.IndexOf(needle);

            if (pos < 0)
            {
                return haystack;
            }

            return haystack.Substring(0, pos) + replace + haystack.Substring(pos + needle.Length);
        }
    }
}