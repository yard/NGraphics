using NGraphics.Codes;
using NGraphics.Models;
using NGraphics.Models.Brushes;
using NGraphics.Models.Elements;

namespace NGraphics
{
    public class Text : Element
    {
        public Rect Frame;
        public string String;
        public TextAlignment Alignment;
        public Font Font;

        public Text(string text, Rect frame, Font font, TextAlignment alignment = TextAlignment.Left, Pen pen = null, BaseBrush brush = null)
            : base(pen, brush)
        {
            String = text;
            Frame = frame;
            Font = font;
            Alignment = alignment;
        }

        protected override void DrawElement(ICanvas canvas)
        {
            canvas.DrawText(String, Frame, Font, Alignment, Pen, Brush);
        }
    }
}
