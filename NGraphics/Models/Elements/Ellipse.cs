using System.Globalization;
using NGraphics.Custom.Interfaces;
using NGraphics.Custom.Models.Brushes;

namespace NGraphics.Custom.Models.Elements
{
    public class Ellipse : Element
    {
        public Ellipse(Rect frame, Pen pen = null, BaseBrush baseBrush = null)
            : base(pen, baseBrush)
        {
            this.frame = frame;
        }

        public Ellipse(Point position, Size size, Pen pen = null, BaseBrush baseBrush = null)
            : this(new Rect(position, size), pen, baseBrush)
        {
        }

        public Ellipse(Point position, double diameter)
            : this(position, new Size(diameter))
        {
        }

        public Ellipse(double diameter)
            : this(Point.Zero, new Size(diameter))
        {
        }

        private readonly Rect frame;

        protected override void DrawElement(ICanvas canvas)
        {
            canvas.DrawEllipse(frame, Pen, Brush);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Ellipse ({0})", frame);
        }
    }
}