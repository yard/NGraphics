using System.Collections.Generic;
using NGraphics.Custom.Interfaces;
using System.Linq;

namespace NGraphics.Custom.Models.Elements {

	/// <summary>
	/// Group.
	/// </summary>
    public class Group : Element {

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Elements.Group"/> class.
		/// </summary>
        public Group() : base(null, null) {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Elements.Group"/> class.
		/// </summary>
		/// <param name="children">Children.</param>
		protected Group(List<IDrawable> children) : base(null, null) {
			Children = children;
		}

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public override IDrawable Clone() {
			return new Group(Children.Select(c => c.Clone()).ToList());
		}

		/// <summary>
		/// The children.
		/// </summary>
        public readonly List<IDrawable> Children = new List<IDrawable>();

		/// <summary>
		/// Draws the element.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
        protected override void DrawElement(ICanvas canvas) {
			canvas.Transform(Transform);

            foreach (var c in Children) {
                c.Draw(canvas);
            }

			canvas.Transform(Transform.GetInverse());
        }
    }
}