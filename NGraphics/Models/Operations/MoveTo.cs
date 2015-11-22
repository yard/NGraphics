namespace NGraphics.Custom.Models.Operations {

	/// <summary>
	/// Move to.
	/// </summary>
    public class MoveTo : PathOperation {

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Operation.MoveTo"/> class.
		/// </summary>
		/// <param name="point">Point.</param>
		/// <param name="isAbsolute">If set to <c>true</c> is absolute.</param>
        public MoveTo(Point point, bool isAbsolute) {
            Start = point;
            IsAbsolute = isAbsolute;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Operations.MoveTo"/> class.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="isAbsolute">If set to <c>true</c> is absolute.</param>
        public MoveTo(double x, double y, bool isAbsolute) : this(new Point(x, y), isAbsolute) {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Operation.MoveTo"/> class.
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		/// <param name="isAbsolute">If set to <c>true</c> is absolute.</param>
        public MoveTo(Point start, Point end, bool isAbsolute) {
            Start = start;
            End = end;
            IsAbsolute = isAbsolute;
        }
			
		/// <summary>
		/// Clone this instance.
		/// </summary>
		public override PathOperation Clone() {
			return new MoveTo(Start, End, IsAbsolute);
		}

		/// <summary>
		/// The end.
		/// </summary>
        public Point End;

		/// <summary>
		/// The is absolute.
		/// </summary>
        public bool IsAbsolute;

		/// <summary>
		/// The start.
		/// </summary>
        public Point Start;
    }
}