using System;
using NGraphics.Custom.Interfaces;
using NGraphics.Custom.Models.Brushes;
using NGraphics.Custom.Models.Transforms;

namespace NGraphics.Custom.Models.Elements {


	/// <summary>
	/// Element.
	/// </summary>
    public abstract class Element : IDrawable {

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
        public string Id { get; set; }

		/// <summary>
		/// Gets or sets the transform.
		/// </summary>
		/// <value>The transform.</value>
		public Transform Transform { get; set; } = Transform.Identity;

		/// <summary>
		/// Gets or sets the pen.
		/// </summary>
		/// <value>The pen.</value>
        public Pen Pen { get; set; }

		/// <summary>
		/// Gets or sets the brush.
		/// </summary>
		/// <value>The brush.</value>
        public BaseBrush Brush { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Elements.Element"/> class.
		/// </summary>
		/// <param name="pen">Pen.</param>
		/// <param name="brush">Brush.</param>
        public Element(Pen pen, BaseBrush brush) {
            Id = Guid.NewGuid().ToString();
            Pen = pen;
            Brush = brush;
            Transform = Transform.Identity;
        }

		/// <summary>
		/// Draws the element.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
        protected abstract void DrawElement(ICanvas canvas);

        #region IDrawable implementation

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public abstract IDrawable Clone();

		/// <summary>
		/// Tint the specified color.
		/// </summary>
		/// <param name="color">Color.</param>
		public virtual IDrawable Tint(Color color) {
			var tinted = (Element)Clone();

			if (tinted.Brush is SolidBrush) {
				((SolidBrush)tinted.Brush).Color = color;
			}

			if (tinted.Pen is Pen) {
				((Pen)tinted.Pen).Color = color;
			}

			return tinted;
		}

		/// <summary>
		/// Draw the specified canvas.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
        public void Draw(ICanvas canvas) {
            var t = Transform;
            var pushedState = false;
            try
            {
                if (t != Transform.Identity)
                {
                    canvas.SaveState();
                    pushedState = true;
                    canvas.Transform(t);
                }
                DrawElement(canvas);
            }
            finally
            {
                if (pushedState)
                {
                    canvas.RestoreState();
                }
            }
        }

        #endregion
    }
}