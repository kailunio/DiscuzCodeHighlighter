using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace DiscuzCodeHighlighter
{
    public class LanguageBase
    {

        protected readonly Regex MultiLineCComments = new Regex(@"/\*[\s\S]*?\*/",
            RegexOptions.Compiled | RegexOptions.Singleline);

        protected readonly Regex SingleLineCComments = new Regex(@"//.*?(?=" + Environment.NewLine + ")", 
            RegexOptions.Compiled | RegexOptions.Multiline);

        protected readonly Regex SingleLinePerlComments = new Regex(@"#.*?(?="+ Environment.NewLine + ")",
            RegexOptions.Compiled | RegexOptions.Multiline);

        protected readonly Regex DoubleQuotedString = new Regex(@"""([^\""\n]|\.)*""", 
            RegexOptions.Compiled );

        protected readonly Regex SingleQuotedString = new Regex(@"'([^\'\n]|\.)*'",
            RegexOptions.Compiled );

        Dictionary<string, Color> _colors = new Dictionary<string, Color>
            {
                { "Comment", Colors.Green },
                { "String", Colors.Brown },
                { "Keyword", Colors.Blue },
            };

        public virtual Dictionary<string, Color> ColorPlans
        {
            get
            {
                return _colors;
            }
        }

        public virtual Dictionary<Regex, string> Regexes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected LanguageBase()
        {
        }

    }
}
