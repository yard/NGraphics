using System.Globalization;
using NGraphics.Custom.Interfaces;
using NGraphics.Custom.Models.Brushes;

namespace NGraphics.Custom.Models.Elements {

	/// <summary>
	/// Rectangle.
	/// </summary>
    public class Rectangle : Element {

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Elements.Rectangle"/> class.
		/// </summary>
		/// <param name="frame">Frame.</param>
		/// <param name="pen">Pen.</param>
		/// <param name="baseBrush">Base brush.</param>
        public Rectangle(Rect frame, Pen pen = null, BaseBrush baseBrush = null) : base(pen, baseBrush) {
            this.frame = frame;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Elements.Rectangle"/> class.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="size">Size.</param>
		/// <param name="pen">Pen.</param>
		/// <param name="baseBrush">Base brush.</param>
        public Rectangle(Point position, Size size, Pen pen = null, BaseBrush baseBrush = null) : this(new Rect(position, size), pen, baseBrush) {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Elements.Rectangle"/> class.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="size">Size.</param>
        public Rectangle(Point position, double size) : this(position, new Size(size)) {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Elements.Rectangle"/> class.
		/// </summary>
		/// <param name="size">Size.</param>
        public Rectangle(double size) : this(Point.Zero, new Size(size)) {
        }

        private readonly Rect frame;

		/// <summary>
		/// Draws the element.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
        protected override void DrawElement(ICanvas canvas) {
			canvas.DrawRectangle(frame, Pen.Clone(), Brush.Clone());
        }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="NGraphics.Custom.Models.Elements.Rectangle"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="NGraphics.Custom.Models.Elements.Rectangle"/>.</returns>
        public override string ToString() {
            return string.Format(CultureInfo.InvariantCulture, "Rectangle ({0})", frame);
        }

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public override IDrawable Clone() {
			return new Rectangle (frame, Pen, Brush) {
				Id = this.Id,
				Transform = this.Transform
			};
		}
    }
}