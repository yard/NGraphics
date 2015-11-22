namespace NGraphics.Custom.Models.Operations {

	/// <summary>
	/// Line to.
	/// </summary>
    public class LineTo : PathOperation {

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Operations.LineTo"/> class.
		/// </summary>
		/// <param name="point">Point.</param>
        public LineTo(Point point) {
            Start = point;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Operations.LineTo"/> class.
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
        public LineTo(Point start, Point end) {
            Start = start;
            End = end;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Operations.LineTo"/> class.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="isAbsolute">If set to <c>true</c> is absolute.</param>
        public LineTo(double x, double y, bool isAbsolute) : this(new Point(x, y, isAbsolute)) {
        }

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public override PathOperation Clone() {
			return new LineTo(Start, End);
		}

		/// <summary>
		/// The end.
		/// </summary>
        public Point End;

		/// <summary>
		/// The start.
		/// </summary>
        public Point Start;
    }
}