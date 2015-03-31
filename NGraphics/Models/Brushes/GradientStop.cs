namespace NGraphics.Models.Brushes
{
    public class GradientStop
    {
        public GradientStop()
        {
        }

        public GradientStop(double offset, Color color)
        {
            Offset = offset;
            Color = color;
        }

        public Color Color;
        public double Offset;
    }
}