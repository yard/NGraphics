using NGraphics.Interfaces;

namespace NGraphics.Models
{
    public class Text : Element
    {
        public Text(string text, Rect frame, Font font, TextAlignment alignment = TextAlignment.Left, Pen pen = null,
            BaseBrush baseBrush = null)
            : base(pen, baseBrush)
        {
            String = text;
            Frame = frame;
            Font = font;
            Alignment = alignment;
        }

        public TextAlignment Alignment;
        public Font Font;
        public Rect Frame;
        public string String;

        protected override void DrawElement(ICanvas canvas)
        {
            canvas.DrawText(String, Frame, Font, Alignment, Pen, BaseBrush);
        }
    }
}