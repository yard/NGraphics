namespace NGraphics.Models
{
    public class Pen
    {
        public Pen()
        {
            Color = Colors.Black;
            Width = 1;
        }

        public Pen(Color color, double width = 1.0)
        {
            Color = color;
            Width = width;
        }

        public Pen(string colorString, double width = 1.0)
            : this(new Color(colorString), width)
        {
        }

        public Color Color;
        public double Width;

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