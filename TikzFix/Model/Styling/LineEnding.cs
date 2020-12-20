using System;

namespace TikzFix.Model.Styling
{
    public enum LineEnding
    {
        NONE,
        START,
        END,
        BOTH
    }

    public static class LineEndingExt
    {
        public static string GetLineEndingTikz(this LineEnding lineEnding)
        {
            return lineEnding switch
            {
                LineEnding.NONE => "-",
                LineEnding.BOTH => "<->",
                LineEnding.END => "->",
                LineEnding.START => "<-",
                _ => throw new ArgumentException("LineEnding cannot be converted"),
            };
        }
    }
}
