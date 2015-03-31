namespace NGraphics.Models.Brushes
{
    public class LinearGradientBrush : GradientBrush
    {
        public LinearGradientBrush()
        {
        }

        public LinearGradientBrush(Point relStart, Point relEnd, params GradientStop[] stops)
        {
            RelativeStart = relStart;
            RelativeEnd = relEnd;
            Stops.AddRange(stops);
        }

        public LinearGradientBrush(Point relStart, Point relEnd, Color startColor, Color endColor)
        {
            RelativeStart = relStart;
            RelativeEnd = relEnd;
            Stops.Add(new GradientStop(0, startColor));
            Stops.Add(new GradientStop(1, endColor));
        }

        public LinearGradientBrush(Point relStart, Point relEnd, Color startColor, Color midColor, Color endColor)
        {
            RelativeStart = relStart;
            RelativeEnd = relEnd;
            Stops.Add(new GradientStop(0, startColor));
            Stops.Add(new GradientStop(0.5, midColor));
            Stops.Add(new GradientStop(1, endColor));
        }

        public Point RelativeEnd;
        public Point RelativeStart;
    }
}