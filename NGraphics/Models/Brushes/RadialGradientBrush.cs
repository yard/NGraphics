namespace NGraphics.Models.Brushes
{
    public class RadialGradientBrush : GradientBrush
    {
        public RadialGradientBrush()
        {
        }

        public RadialGradientBrush(Point relCenter, double relRadius, params GradientStop[] stops)
        {
            RelativeCenter = relCenter;
            RelativeFocus = relCenter;
            RelativeRadius = relRadius;
            Stops.AddRange(stops);
        }

        public RadialGradientBrush(Point relCenter, double relRadius, Color startColor, Color endColor)
        {
            RelativeCenter = relCenter;
            RelativeFocus = relCenter;
            RelativeRadius = relRadius;
            Stops.Add(new GradientStop(0, startColor));
            Stops.Add(new GradientStop(1, endColor));
        }

        public RadialGradientBrush(Point relCenter, double relRadius, Color startColor, Color midColor, Color endColor)
        {
            RelativeCenter = relCenter;
            RelativeFocus = relCenter;
            RelativeRadius = relRadius;
            Stops.Add(new GradientStop(0, startColor));
            Stops.Add(new GradientStop(0.5, midColor));
            Stops.Add(new GradientStop(1, endColor));
        }

        public Point RelativeCenter;
        public Point RelativeFocus;
        public double RelativeRadius;
    }
}