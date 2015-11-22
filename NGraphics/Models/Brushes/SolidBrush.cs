using NGraphics.Custom.Codes;

namespace NGraphics.Custom.Models.Brushes
{
	/// <summary>
	/// Solid brush.
	/// </summary>
    public class SolidBrush : BaseBrush {

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Brushes.SolidBrush"/> class.
		/// </summary>
        public SolidBrush() {
            Color = Colors.Black;
            FillMode = FillMode.NonZero;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Brushes.SolidBrush"/> class.
		/// </summary>
		/// <param name="color">Color.</param>
        public SolidBrush(Color color) {
            Color = color;
            FillMode = FillMode.NonZero;
        }

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public override BaseBrush Clone() {
			return new SolidBrush {
				Color = this.Color,
				FillMode = this.FillMode
			};
		}

		/// <summary>
		/// The color.
		/// </summary>
        public Color Color;

		/// <summary>
		/// The fill mode.
		/// </summary>
        public FillMode FillMode;
    }
}