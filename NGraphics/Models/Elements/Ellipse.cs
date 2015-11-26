using System.Globalization;
using NGraphics.Custom.Interfaces;
using NGraphics.Custom.Models.Brushes;

namespace NGraphics.Custom.Models.Elements {
	
	/// <summary>
	/// Ellipse.
	/// </summary>
    public class Ellipse : Element {
		
		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Elements.Ellipse"/> class.
		/// </summary>
		/// <param name="frame">Frame.</param>
		/// <param name="pen">Pen.</param>
		/// <param name="baseBrush">Base brush.</param>
        public Ellipse(Rect frame, Pen pen = null, BaseBrush baseBrush = null) : base(pen, baseBrush) {
            this.frame = frame;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Elements.Ellipse"/> class.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="size">Size.</param>
		/// <param name="pen">Pen.</param>
		/// <param name="baseBrush">Base brush.</param>
        public Ellipse(Point position, Size size, Pen pen = null, BaseBrush baseBrush = null) : this(new Rect(position, size), pen, baseBrush) {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Elements.Ellipse"/> class.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="diameter">Diameter.</param>
        public Ellipse(Point position, double diameter) : this(position, new Size(diameter)) {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Elements.Ellipse"/> class.
		/// </summary>
		/// <param name="diameter">Diameter.</param>
        public Ellipse(double diameter) : this(Point.Zero, new Size(diameter)) {
        }

		/// <summary>
		/// The frame.
		/// </summary>
        private readonly Rect frame;

		/// <summary>
		/// Draws the element.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
        protected override void DrawElement(ICanvas canvas) {
            canvas.DrawEllipse(frame, Pen, Brush);
        }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="NGraphics.Custom.Models.Elements.Ellipse"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="NGraphics.Custom.Models.Elements.Ellipse"/>.</returns>
        public override string ToString() {
            return string.Format(CultureInfo.InvariantCulture, "Ellipse ({0})", frame);
        }

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public override IDrawable Clone() {
			return new Ellipse(frame, Pen.Clone(), Brush.Clone()) {
				Id = this.Id,
				Transform = this.Transform
			};
		}
    }
}