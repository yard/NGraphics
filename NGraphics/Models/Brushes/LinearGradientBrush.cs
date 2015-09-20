namespace NGraphics.Custom.Models.Brushes
{
    public class LinearGradientBrush : GradientBrush
    {
        public Point Start;
        public Point End;
        public bool Absolute = false;

        public LinearGradientBrush()
        {
        }
        public LinearGradientBrush(Point relStart, Point relEnd, params GradientStop[] stops)
        {
            Start = relStart;
            End = relEnd;
            Stops.AddRange(stops);
        }
        public LinearGradientBrush(Point relStart, Point relEnd, Color startColor, Color endColor)
        {
            Start = relStart;
            End = relEnd;
            Stops.Add(new GradientStop(0, startColor));
            Stops.Add(new GradientStop(1, endColor));
        }
        public LinearGradientBrush(Point relStart, Point relEnd, Color startColor, Color midColor, Color endColor)
        {
            Start = relStart;
            End = relEnd;
            Stops.Add(new GradientStop(0, startColor));
            Stops.Add(new GradientStop(0.5, midColor));
            Stops.Add(new GradientStop(1, endColor));
        }
        public Point GetAbsoluteStart(Rect frame)
        {
            if (Absolute) return Start;
            return frame.TopLeft + Start * frame.Size;
        }
        public Point GetAbsoluteEnd(Rect frame)
        {
            if (Absolute) return End;
            return frame.TopLeft + End * frame.Size;
        }
    }
}