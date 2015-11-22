using System.Collections.Generic;
using System.IO;
using System.Linq;
using NGraphics.Custom.Interfaces;
using NGraphics.Custom.Models;
using NGraphics.Custom.Parsers;
using NGraphics.Custom.Models.Transforms;

namespace NGraphics.Custom {
	
    public class Graphic : IDrawable {

		/// <summary>
		/// Gets or sets the transform.
		/// </summary>
		/// <value>The transform.</value>
		public Transform Transform { get; set; } = Transform.Identity;

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Graphic"/> class.
		/// </summary>
		/// <param name="size">Size.</param>
		/// <param name="viewBox">View box.</param>
        public Graphic(Size size, Rect viewBox) {
            Size = size;
            ViewBox = viewBox;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Graphic"/> class.
		/// </summary>
		/// <param name="size">Size.</param>
        public Graphic(Size size) : this(size, new Rect(Point.Zero, size)) {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Graphic"/> class.
		/// </summary>
		/// <param name="children">Children.</param>
		protected Graphic(List<IDrawable> children) {
			Children = children;
		}

		/// <summary>
		/// The children.
		/// </summary>
        public readonly List<IDrawable> Children = new List<IDrawable>();

		/// <summary>
		/// The description.
		/// </summary>
        public string Description = "";

		/// <summary>
		/// The size.
		/// </summary>
        public Size Size;

		/// <summary>
		/// The title.
		/// </summary>
        public string Title = "";

		/// <summary>
		/// The view box.
		/// </summary>
        public Rect ViewBox;

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public IDrawable Clone() {
			return new Graphic(Children.Select(child => child.Clone()).ToList()) {
				Description = this.Description,
				Title = this.Title,
				Size = this.Size,
				ViewBox = this.ViewBox
			};
		}

		/// <summary>
		/// Draw the specified canvas.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
        public void Draw(ICanvas canvas) {
            canvas.SaveState();

            //
            // Scale the viewBox into the size
            //
            var sx = 1.0;
            if (ViewBox.Width > 0)
            {
                sx = Size.Width/ViewBox.Width;
            }
            var sy = 1.0;
            if (ViewBox.Height > 0)
            {
                sy = Size.Height/ViewBox.Height;
            }

            canvas.Scale(sx, sy);
            canvas.Translate(-ViewBox.X, -ViewBox.Y);

			canvas.Transform(Transform);

            foreach (var c in Children) {
                c.Draw(canvas);
            }

			canvas.Transform(Transform.GetInverse());

            canvas.RestoreState();
        }

		/// <summary>
		/// Loads the svg.
		/// </summary>
		/// <returns>The svg.</returns>
		/// <param name="reader">Reader.</param>
        public static Graphic LoadSvg(TextReader reader) {
          var valuesParser = new ValuesParser();
            var svgr = new SvgReader(reader, new StylesParser(valuesParser),valuesParser);
            return svgr.Graphic;
        }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="NGraphics.Custom.Graphic"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="NGraphics.Custom.Graphic"/>.</returns>
        public override string ToString() {
            try
            {
                if (Children.Count == 0)
                    return "Graphic";
                var w =
                    Children.
                        GroupBy(x => x.GetType().Name).
                        Select(x => x.Count() + " " + x.Key);
                return "Graphic with " + string.Join(", ", w);
            }
            catch
            {
                return "Graphic with errors!";
            }
        }
    }
}