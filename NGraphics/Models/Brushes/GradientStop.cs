namespace NGraphics.Custom.Models.Brushes {

	/// <summary>
	/// Gradient stop.
	/// </summary>
    public class GradientStop {

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Brushes.GradientStop"/> class.
		/// </summary>
        public GradientStop() {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Brushes.GradientStop"/> class.
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <param name="color">Color.</param>
        public GradientStop(double offset, Color color) {
            Offset = offset;
            Color = color;
        }

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public GradientStop Clone() {
			return new GradientStop {
				Offset = this.Offset,
				Color = this.Color
			};
		}

		/// <summary>
		/// The color.
		/// </summary>
        public Color Color;

		/// <summary>
		/// The offset.
		/// </summary>
        public double Offset;
    }
}