using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Documents;

namespace DiscuzCodeHighlighter
{
    public class CodeBox : RichTextBox
    {

        bool _isToRender = false;
        bool _isUbbView = false;
        string _code = string.Empty;

        /// <summary>
        /// 当前是为UBB视图
        /// </summary>
        public bool IsUbbView
        {
            get
            {
                return _isUbbView;
            }
            set
            {
                if (_isUbbView != value)
                {
                    if (this.ViewChanged != null)
                        this.ViewChanged(this, value);

                    _isUbbView = value;
                }
            }
        }

        /// <summary>
        /// 存放的代码
        /// </summary>
        public string Code
        {
            get
            {
                return _code;
            }
            private set
            {
                _code = value;
                if (this.CodeChanged != null)
                    this.CodeChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// 视图发生变化
        /// </summary>
        public event ViewChangedEventHandler ViewChanged;

        /// <summary>
        /// 代码发生改变
        /// </summary>
        public event EventHandler CodeChanged;


        /// <summary>
        /// 设置CodeBox显示内容
        /// </summary>
        /// <param name="paragraph">待显示的段落</param>
        public void SetContent(Paragraph paragraph)
        {
            _isToRender = true;

            this.Document.Blocks.Clear();
            this.Document.Blocks.Add(paragraph);

            _isToRender = false;
        }

        /// <summary>
        /// 重写OnTextChanged方法，在某些情况中改变Code属性的值
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if (!_isUbbView && !_isToRender)
            {
                var textRange = new TextRange(
                    this.Document.ContentStart, this.Document.ContentEnd);
                this.Code = textRange.Text;
            }
        }

        /// <summary>
        /// 重写OnPreviewKeyDown方法，重写定义Enter与CtrlV的行为
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers != ModifierKeys.Shift)
            {
                this.CaretPosition.InsertLineBreak();
                this.CaretPosition = this.CaretPosition.GetNextInsertionPosition(LogicalDirection.Forward);
                e.Handled = true;
            }
            else if (e.Key == Key.V && Keyboard.Modifiers == ModifierKeys.Control)
            {
                var text = Clipboard.GetText();
                text = text.Replace("\t", "    ");

                var paragraph = new Paragraph();
                paragraph.Inlines.Add(text);
                this.SetContent(paragraph);
                
                this.Code = text;

                e.Handled = true;
            }
            else
            {
                base.OnPreviewKeyDown(e);
            }
        }

        /// <summary>
        /// 重写OnPreviewMouseRightButtonUp，禁止右键菜单
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
