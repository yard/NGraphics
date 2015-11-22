using System.Collections.Generic;

namespace NGraphics.Custom.Models.Brushes {

	/// <summary>
	/// Gradient brush.
	/// </summary>
    public abstract class GradientBrush : BaseBrush {
		
		/// <summary>
		/// The stops.
		/// </summary>
        public readonly List<GradientStop> Stops = new List<GradientStop>();

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Brushes.GradientBrush"/> class.
		/// </summary>
		/// <param name="stops">Stops.</param>
		protected GradientBrush(List<GradientStop> stops) {
			Stops = stops;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Brushes.GradientBrush"/> class.
		/// </summary>
		public GradientBrush() {
		}

		/// <summary>
		/// Adds the stop.
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <param name="color">Color.</param>
        public void AddStop(double offset, Color color) {
            Stops.Add(new GradientStop(offset, color));
        }

    }
}