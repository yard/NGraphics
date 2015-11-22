namespace NGraphics.Custom.Models.Operations {

	/// <summary>
	/// Curve to.
	/// </summary>
    public class CurveTo : PathOperation {

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Operations.CurveTo"/> class.
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="firstControlPoint">First control point.</param>
		/// <param name="secondControlPoint">Second control point.</param>
        public CurveTo(Point start, Point firstControlPoint, Point secondControlPoint) {
            Start = start;
            FirstControlPoint = firstControlPoint;
            SecondControlPoint = secondControlPoint;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Operations.CurveTo"/> class.
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="firstControlPoint">First control point.</param>
		/// <param name="secondControlPoint">Second control point.</param>
		/// <param name="end">End.</param>
        public CurveTo(Point start, Point firstControlPoint, Point secondControlPoint, Point end) {
            Start = start;
            FirstControlPoint = firstControlPoint;
            SecondControlPoint = secondControlPoint;
            End = end;
        }

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public override PathOperation Clone() {
			return new CurveTo(Start, FirstControlPoint, SecondControlPoint, End);
		}

		/// <summary>
		/// The end.
		/// </summary>
        public Point End;

		/// <summary>
		/// The first control point.
		/// </summary>
        public Point FirstControlPoint;

		/// <summary>
		/// The second control point.
		/// </summary>
        public Point SecondControlPoint;

		/// <summary>
		/// The start.
		/// </summary>
        public Point Start;
    }
}