using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DiscuzCodeHighlighter.Languages
{
    public class LangCSharp : LanguageBase
    {
        Dictionary<Regex, string> _regexes;

        public override Dictionary<Regex, string> Regexes
        {
            get { return _regexes; }
        }

        string Keywords = "abstract|as|base|bool|break|byte|case|catch|char|checked|class|const|" +
                    "continue|decimal|default|delegate|do|double|else|enum|event|explicit|" +
                    "extern|false|finally|fixed|float|for|foreach|get|goto|if|implicit|in|int|" +
                    "interface|internal|is|lock|long|namespace|new|null|object|operator|out|" +
                    "override|params|private|protected|public|readonly|ref|return|sbyte|sealed|set|" +
                    "short|sizeof|stackalloc|static|string|struct|switch|this|throw|true|try|" +
                    "typeof|uint|ulong|unchecked|unsafe|ushort|using|virtual|void|while"
                    // 似乎var也很重要
                    + "|var";

        public LangCSharp()
        {
            _regexes = new Dictionary<Regex, string>
            {
                { MultiLineCComments, "Comment" },
                { SingleLineCComments, "Comment" },
                { new Regex(@"@""([^\""\n]|\.)*""", RegexOptions.Compiled ), "String" },
                { DoubleQuotedString, "String" },
                { new Regex(@"\b(" + Keywords + @")\b", RegexOptions.Compiled | RegexOptions.Singleline ), "Keyword" },
            };
        }
    }
}