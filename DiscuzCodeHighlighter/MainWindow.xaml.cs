using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using DiscuzCodeHighlighter.Languages;

namespace DiscuzCodeHighlighter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string _text = null;

        // 暂存UBB代码
        string _codeDiscuz;

        //  暂存渲染后的RTF段落
        Paragraph _codeParagraph;

        public MainWindow()
        {
            _codeDiscuz = string.Empty;
            _codeParagraph = new Paragraph();

            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            // 语言
            var lang = new LangCSharp();
            
            // 高亮工具
            var lighter = new Highlighter();

            // 解析代码
            var pieces = lighter.Process(textCode.Code, lang);

            RenderCode(textCode.Code, pieces, lang);

            // 显示结果
            textCode.SetContent(_codeParagraph);
        }

        void RenderCode(string code, List<HighlightPiece> pieces, LanguageBase lang)
        {
            var paragraph = new Paragraph();

            int index = 0;
            var sb = new StringBuilder();
            sb.Append("[font=Consolas]");
            for (int i = 0; i < pieces.Count; i++)
            {
                var piece = pieces[i];

                // 每个高亮之前的部分
                var pieceCur = code.Substring(index, piece.Index - index);
                paragraph.Inlines.Add(pieceCur);
                sb.Append(pieceCur);

                // 高亮的代码块
                var span = new Span();
                var color = lang.ColorPlans[piece.Style];
                span.Foreground = new SolidColorBrush(color);
                pieceCur = code.Substring(piece.Index, piece.Length);
                span.Inlines.Add(pieceCur);
                paragraph.Inlines.Add(span);
                sb.Append(string.Format(
                    "[color=#{0:x2}{1:x2}{2:x2}]{3}[/color]", color.R, color.G, color.B, pieceCur)
                );

                index = piece.Index + piece.Length;

                // 最后的部分
                if (i == pieces.Count - 1)
                {
                    pieceCur = code.Substring(index, code.Length - index);
                    paragraph.Inlines.Add(pieceCur);
                    sb.Append(pieceCur);
                }
            }
            sb.Append("[/font]");
            _codeDiscuz = sb.ToString();
            _codeParagraph = paragraph;
        }

        private void radioUbb_Checked(object sender, RoutedEventArgs e)
        {
            textCode.IsUbbView = true;
        }

        private void checkUbb_Unchecked(object sender, RoutedEventArgs e)
        {
            textCode.IsUbbView = false;
        }

        private void textCode_ViewChanged(object sender, bool isUbb)
        {
            if (isUbb)
            {
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(_codeDiscuz);

                textCode.SetContent(paragraph);
            }
            else
            {
                textCode.SetContent(_codeParagraph);
            }
        }

        private void btnCopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(_codeDiscuz);
                MessageBox.Show("已复制到剪贴板中!");
            }
            catch
            {
                MessageBox.Show("访问剪贴板时遇到异常，复制失败");
            }
        }
    }
}
