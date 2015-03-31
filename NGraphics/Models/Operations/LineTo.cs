namespace NGraphics.Models.Operations
{
    public class LineTo : PathOperation
    {
        public LineTo(Point point)
        {
            Start = point;
        }

        public LineTo(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public LineTo(double x, double y, bool isAbsolute)
            : this(new Point(x, y, isAbsolute))
        {
        }

        public Point End;
        public Point Start;
    }
}