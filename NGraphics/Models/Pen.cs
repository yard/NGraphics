using NGraphics.Custom.Codes;

namespace NGraphics.Custom.Models {

	/// <summary>
	/// Pen.
	/// </summary>
    public class Pen {

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Pen"/> class.
		/// </summary>
        public Pen() {
            Color = Colors.Black;
            Width = 1;
            LineJoin = SvgStrokeLineJoin.Miter;
            LineCap = SvgStrokeLineCap.Square;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Pen"/> class.
		/// </summary>
		/// <param name="color">Color.</param>
		/// <param name="width">Width.</param>
        public Pen(Color color, double width = 1.0) {
            Color = color;
            Width = width;
            LineJoin = SvgStrokeLineJoin.Miter;
            LineCap = SvgStrokeLineCap.Square;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Pen"/> class.
		/// </summary>
		/// <param name="colorString">Color string.</param>
		/// <param name="width">Width.</param>
        public Pen(string colorString, double width = 1.0) : this(new Color(colorString), width)
        {
        }

		/// <summary>
		/// The color.
		/// </summary>
        public Color Color;

		/// <summary>
		/// The width.
		/// </summary>
        public double Width;

		/// <summary>
		/// The line join.
		/// </summary>
        public SvgStrokeLineJoin LineJoin;

		/// <summary>
		/// The line cap.
		/// </summary>
        public SvgStrokeLineCap LineCap;

		/// <summary>
		/// Withs the width.
		/// </summary>
		/// <returns>The width.</returns>
		/// <param name="width">Width.</param>
        public Pen WithWidth(double width) {
            return new Pen(Color, width);
        }

		/// <summary>
		/// Withs the color.
		/// </summary>
		/// <returns>The color.</returns>
		/// <param name="color">Color.</param>
        public Pen WithColor(Color color) {
            return new Pen(color, Width);
        }

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public Pen Clone() {
			return new Pen {
				Color = this.Color,
				Width = this.Width,
				LineCap = this.LineCap,
				LineJoin = this.LineJoin
			};
		}
    }
}