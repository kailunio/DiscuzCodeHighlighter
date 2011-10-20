using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DiscuzCodeHighlighter
{
    public class HighlightPiece
    {
        public string Style { get; private set; }
        public int Index { get; private set; }
        public int Length { get; private set; }

        public HighlightPiece(string style, Match match)
        {
            this.Style = style;
            this.Index = match.Index;
            this.Length = match.Length;
         }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", this.Style, this.Index, this.Length); 
        }
    }
}
