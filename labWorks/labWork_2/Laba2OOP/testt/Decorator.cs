using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testt
{
    

    public abstract class TextComponent
        {
            protected  TextComponent _textComponent;
            public abstract string GetText();

            public abstract ConsoleColor GetColor();
            public abstract string GetFormat();
            public abstract string GetRawText();
            public abstract TextComponent Clone();
            public abstract void SetFormat(string format); 

    }

        public class PlainText : TextComponent
        {
            private readonly string _text;
            private string _format;

            public PlainText(string text)
            {
                _text = text;
                _format = "Plain";
             }

            public override string GetText() => _text;
            public override ConsoleColor GetColor() => AppSettings.Instance.TextColor;
            public override string GetFormat() => _format;
            public override string GetRawText() => _text;
            public override TextComponent Clone()
            {
                return new PlainText(this._text);
            }
            public override void SetFormat(string format)
            {
                _format = format;
             }

            

    }

    public abstract class TextDecorator : TextComponent
        {
     

        protected TextDecorator(TextComponent textComponent)
        {
            _textComponent = textComponent;
        }

        public override void SetFormat(string format)
        {
            _textComponent.SetFormat(format);
        }
      
            public override string GetText() => _textComponent.GetText();
            public override ConsoleColor GetColor() => _textComponent.GetColor();
            public override string GetFormat() => _textComponent.GetFormat();
            public override string GetRawText() => _textComponent.GetRawText();
        }

    public class BoldText : TextDecorator
    {
        public BoldText(TextComponent textComponent) : base(textComponent) { }
       
        public override void SetFormat(string format)
        {
            base.SetFormat(format + ",Bold");
        }
        public override ConsoleColor GetColor() => ConsoleColor.Yellow;
        public override string GetFormat() => _textComponent.GetFormat() + ",Bold";
        public override string GetText() => _textComponent.GetText(); 
        public override string GetRawText() => _textComponent.GetRawText();
        public override TextComponent Clone()
        {
            return new BoldText(_textComponent.Clone());
        }
    }

    public class ItalicText : TextDecorator
    {
        public ItalicText(TextComponent textComponent) : base(textComponent) { }
        public override void SetFormat(string format)
        {
            base.SetFormat(format + ",Italic");
        }
        public override ConsoleColor GetColor() => ConsoleColor.Cyan;
        public override string GetFormat() => _textComponent.GetFormat() + ",Italic";
        public override string GetText() => _textComponent.GetText();
        public override string GetRawText() => _textComponent.GetRawText();
        public override TextComponent Clone()
        {
            return new ItalicText(_textComponent.Clone());
        }
    }

    public class UnderlineText : TextDecorator
    {
        public UnderlineText(TextComponent textComponent) : base(textComponent) { }
        public override void SetFormat(string format)
        {
            base.SetFormat(format + ",Underline");
        }
        public override ConsoleColor GetColor() => ConsoleColor.DarkRed;
        public override string GetFormat() => _textComponent.GetFormat() + ",Underline";
        public override string GetText() => _textComponent.GetText(); 
        public override string GetRawText() => _textComponent.GetRawText();
        public override TextComponent Clone()
        
        {
            return new UnderlineText(_textComponent.Clone());
        }
    }

      public class HeaderText : TextDecorator
      {
         private readonly int _level;

        public HeaderText(TextComponent textComponent, int level) : base(textComponent)
        {
            _level = level;
        }

        public override void SetFormat(string format)
        {
            base.SetFormat(format + $",Header{_level}");
        }

        public override ConsoleColor GetColor() => ConsoleColor.Green;
        public override string GetFormat() => _textComponent.GetFormat() + $",Header{_level}";
        public override string GetText() => _textComponent.GetText();
        public override string GetRawText() => _textComponent.GetRawText();

        public override TextComponent Clone()
        {
            return new HeaderText(_textComponent.Clone(), _level);
        }
    }
    
}
