using NGraphics.Codes;

namespace NGraphics.Models
{
    public class Pen
    {
        public Pen()
        {
            Color = Colors.Black;
            Width = 1;
            LineJoin = SvgStrokeLineJoin.Miter;
        }

        public Pen(Color color, double width = 1.0)
        {
            Color = color;
            Width = width;
            LineJoin = SvgStrokeLineJoin.Miter;
        }

        public Pen(string colorString, double width = 1.0)
            : this(new Color(colorString), width)
        {
        }

        public Color Color;
        public double Width;
        public SvgStrokeLineJoin LineJoin;

        public Pen WithWidth(double width)
        {
            return new Pen(Color, width);
        }

        public Pen WithColor(Color color)
        {
            return new Pen(color, Width);
        }
    }
}