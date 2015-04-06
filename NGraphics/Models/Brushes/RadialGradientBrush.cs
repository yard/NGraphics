namespace NGraphics.Models.Brushes
{
    public class RadialGradientBrush : GradientBrush
    {
        public Point Center;
        public Point Focus;
        public Size Radius;
        public bool Absolute = false;

        public RadialGradientBrush()
        {
        }
        public RadialGradientBrush(Point relCenter, Size relRadius, params GradientStop[] stops)
        {
            Center = relCenter;
            Focus = relCenter;
            Radius = relRadius;
            Stops.AddRange(stops);
        }
        public RadialGradientBrush(Point relCenter, Size relRadius, Color startColor, Color endColor)
        {
            Center = relCenter;
            Focus = relCenter;
            Radius = relRadius;
            Stops.Add(new GradientStop(0, startColor));
            Stops.Add(new GradientStop(1, endColor));
        }
        public RadialGradientBrush(Color startColor, Color endColor)
            : this(new Point(0.5, 0.5), new Size(0.5), startColor, endColor)
        {
        }
        public RadialGradientBrush(Point relCenter, Size relRadius, Color startColor, Color midColor, Color endColor)
        {
            Center = relCenter;
            Focus = relCenter;
            Radius = relRadius;
            Stops.Add(new GradientStop(0, startColor));
            Stops.Add(new GradientStop(0.5, midColor));
            Stops.Add(new GradientStop(1, endColor));
        }
        public RadialGradientBrush(Color startColor, Color midColor, Color endColor)
            : this(new Point(0.5, 0.5), new Size(0.5), startColor, midColor, endColor)
        {
        }
        public Point GetAbsoluteCenter(Rect frame)
        {
            if (Absolute) return Center;
            return frame.TopLeft + Center * frame.Size;
        }
        public Size GetAbsoluteRadius(Rect frame)
        {
            if (Absolute) return Radius;
            return Radius * frame.Size;
        }
        public Point GetAbsoluteFocus(Rect frame)
        {
            if (Absolute) return Focus;
            return frame.TopLeft + Focus * frame.Size;
        }
    }
}