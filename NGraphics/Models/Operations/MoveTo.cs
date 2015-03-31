namespace NGraphics
{
    public class MoveTo : PathOperation
    {
        public MoveTo(Point point, bool isAbsolute)
        {
            Point = point;
            IsAbsolute = isAbsolute;
        }

        public MoveTo(double x, double y, bool isAbsolute)
            : this(new Point(x, y), isAbsolute)
        {
        }

        public MoveTo(Point start, Point end, bool isAbsolute)
        {
            Start = start;
            End = end;
            IsAbsolute = isAbsolute;
        }

        public Point End;
        public bool IsAbsolute;
        public Point Point;
        public Point Start;

        public override Point GetContinueCurveControlPoint()
        {
            return Point;
        }
    }
}