using System;
using System.Windows.Media;

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


        /// <summary>
        /// Converts LineEnding into the PenLineCap
        /// </summary>
        /// <param name="lineEnding"></param>
        /// <returns>
        /// Array of PenLineCaps
        /// Index 0 is Starting Cap
        /// Index 1 is Ending Cap
        /// Index 2 is Dash cap
        /// </returns>
        public static PenLineCap[] GetLineCaps(this LineEnding lineEnding)
        {
            return lineEnding switch
            {
                LineEnding.NONE => new PenLineCap[] { PenLineCap.Flat, PenLineCap.Flat, PenLineCap.Flat },
                LineEnding.BOTH => new PenLineCap[] { PenLineCap.Triangle, PenLineCap.Triangle, PenLineCap.Flat },
                LineEnding.END => new PenLineCap[] { PenLineCap.Flat, PenLineCap.Triangle, PenLineCap.Flat },
                LineEnding.START => new PenLineCap[] { PenLineCap.Triangle, PenLineCap.Flat, PenLineCap.Flat },
                _ => throw new ArgumentException("LineEnding cannot be converted"),
            };
        }

    }
}
