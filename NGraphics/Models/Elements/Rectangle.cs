using System.Globalization;
using NGraphics.Custom.Interfaces;
using NGraphics.Custom.Models.Brushes;

namespace NGraphics.Custom.Models.Elements
{
    public class Rectangle : Element
    {
        public Rectangle(Rect frame, Pen pen = null, BaseBrush baseBrush = null)
            : base(pen, baseBrush)
        {
            this.frame = frame;
        }

        public Rectangle(Point position, Size size, Pen pen = null, BaseBrush baseBrush = null)
            : this(new Rect(position, size), pen, baseBrush)
        {
        }

        public Rectangle(Point position, double size)
            : this(position, new Size(size))
        {
        }

        public Rectangle(double size)
            : this(Point.Zero, new Size(size))
        {
        }

        private readonly Rect frame;

        protected override void DrawElement(ICanvas canvas)
        {
            canvas.DrawRectangle(frame, Pen, Brush);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Rectangle ({0})", frame);
        }
    }
}