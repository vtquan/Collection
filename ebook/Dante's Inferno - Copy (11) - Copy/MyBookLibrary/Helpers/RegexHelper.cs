using System.Text.RegularExpressions;

namespace MyBookLibrary.Helpers
{
    public class RegexHelper
    {
        public static readonly RegexOptions RegexOpts;
        public static readonly RegexOptions RegexOptsSingleline;
        public static readonly RegexOptions RegexOptsMultiline;

        static RegexHelper()
        {
            RegexOpts = RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace |
                        RegexOptions.CultureInvariant;
            RegexOptsSingleline = RegexOptions.Singleline | RegexOpts;
            RegexOptsMultiline = RegexOptions.Multiline | RegexOpts;
        }
    }
}