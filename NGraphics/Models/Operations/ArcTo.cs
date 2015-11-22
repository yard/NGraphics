using System;

namespace NGraphics.Custom.Models.Operations {

	/// <summary>
	/// Arc to.
	/// </summary>
    public class ArcTo : PathOperation {

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Operations.ArcTo"/> class.
		/// </summary>
		/// <param name="radius">Radius.</param>
		/// <param name="largeArc">If set to <c>true</c> large arc.</param>
		/// <param name="sweepClockwise">If set to <c>true</c> sweep clockwise.</param>
		/// <param name="point">Point.</param>
        public ArcTo(Size radius, bool largeArc, bool sweepClockwise, Point point) {
            Radius = radius;
            LargeArc = largeArc;
            SweepClockwise = sweepClockwise;
            Point = point;
        }

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public override PathOperation Clone() {
			return new ArcTo(Radius, LargeArc, SweepClockwise, Point);
		}

		/// <summary>
		/// The large arc.
		/// </summary>
        public bool LargeArc;

		/// <summary>
		/// The point.
		/// </summary>
        public Point Point;

		/// <summary>
		/// The radius.
		/// </summary>
        public Size Radius;

		/// <summary>
		/// The sweep clockwise.
		/// </summary>
        public bool SweepClockwise;

        public void GetCircles(Point prevPoint, out Point circle1Center, out Point circle2Center) {
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