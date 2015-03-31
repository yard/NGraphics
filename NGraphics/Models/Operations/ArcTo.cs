using System;

namespace NGraphics
{
    public class ArcTo : PathOperation
    {
        public ArcTo(Size radius, bool largeArc, bool sweepClockwise, Point point)
        {
            Radius = radius;
            LargeArc = largeArc;
            SweepClockwise = sweepClockwise;
            Point = point;
        }

        public bool LargeArc;
        public Point Point;
        public Size Radius;
        public bool SweepClockwise;

        public override Point GetContinueCurveControlPoint()
        {
            return Point;
        }

        public void GetCircles(Point prevPoint, out Point circle1Center, out Point circle2Center)
        {
            //Following explanation at http://mathforum.org/library/drmath/view/53027.html'
            if (Radius.Width == 0)
                throw new Exception("radius x of zero");
            if (Radius.Height == 0)
                throw new Exception("radius y of zero");
            var p1 = prevPoint;
            var p2 = Point;
            if (p1 == p2)
                throw new Exception("coincident points gives infinite number of Circles");
            // delta x, delta y between points
            var dp = p2 - p1;
            // dist between points
            var q = dp.Distance;
            if (q > 2.0*Radius.Diagonal)
                throw new Exception("separation of points > diameter");
            // halfway point
            var p3 = (p1 + p2)/2;
            // distance along the mirror line
            var xd = Math.Sqrt(Radius.Width*Radius.Width - (q/2)*(q/2));
            var yd = Math.Sqrt(Radius.Height*Radius.Height - (q/2)*(q/2));

            circle1Center = new Point(p3.X - yd*dp.Y/q, p3.Y + xd*dp.X/q);
            circle2Center = new Point(p3.X + yd*dp.Y/q, p3.Y - xd*dp.X/q);
        }
    }
}