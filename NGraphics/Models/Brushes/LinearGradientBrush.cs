using System.Linq;
using System.Collections.Generic;namespace NGraphics.Custom.Models.Brushes {

	/// <summary>
	/// Linear gradient brush.
	/// </summary>
    public class LinearGradientBrush : GradientBrush {

		/// <summary>
		/// The start.
		/// </summary>
        public Point Start;

		/// <summary>
		/// The end.
		/// </summary>
        public Point End;

		/// <summary>
		/// The absolute.
		/// </summary>
        public bool Absolute = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Brushes.LinearGradientBrush"/> class.
		/// </summary>
        public LinearGradientBrush() {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Brushes.LinearGradientBrush"/> class.
		/// </summary>
		protected LinearGradientBrush(List<GradientStop> stops) : base(stops) {
		}

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public override BaseBrush Clone() {
			return new LinearGradientBrush(this.Stops.Select(stop => stop.Clone()).ToList()) {
				Start = this.Start,
				End = this.End,
				Absolute = this.Absolute
			};
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Brushes.LinearGradientBrush"/> class.
		/// </summary>
		/// <param name="relStart">Rel start.</param>
		/// <param name="relEnd">Rel end.</param>
		/// <param name="stops">Stops.</param>
        public LinearGradientBrush(Point relStart, Point relEnd, params GradientStop[] stops) {
            Start = relStart;
            End = relEnd;
            Stops.AddRange(stops);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Brushes.LinearGradientBrush"/> class.
		/// </summary>
		/// <param name="relStart">Rel start.</param>
		/// <param name="relEnd">Rel end.</param>
		/// <param name="startColor">Start color.</param>
		/// <param name="endColor">End color.</param>
        public LinearGradientBrush(Point relStart, Point relEnd, Color startColor, Color endColor) {
            Start = relStart;
            End = relEnd;
            Stops.Add(new GradientStop(0, startColor));
            Stops.Add(new GradientStop(1, endColor));
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Brushes.LinearGradientBrush"/> class.
		/// </summary>
		/// <param name="relStart">Rel start.</param>
		/// <param name="relEnd">Rel end.</param>
		/// <param name="startColor">Start color.</param>
		/// <param name="midColor">Middle color.</param>
		/// <param name="endColor">End color.</param>
        public LinearGradientBrush(Point relStart, Point relEnd, Color startColor, Color midColor, Color endColor) {
            Start = relStart;
            End = relEnd;
            Stops.Add(new GradientStop(0, startColor));
            Stops.Add(new GradientStop(0.5, midColor));
            Stops.Add(new GradientStop(1, endColor));
        }

		/// <summary>
		/// Gets the absolute start.
		/// </summary>
		/// <returns>The absolute start.</returns>
		/// <param name="frame">Frame.</param>
        public Point GetAbsoluteStart(Rect frame) {
            if (Absolute) return Start;
            return frame.TopLeft + Start * frame.Size;
        }

		/// <summary>
		/// Gets the absolute end.
		/// </summary>
		/// <returns>The absolute end.</returns>
		/// <param name="frame">Frame.</param>
        public Point GetAbsoluteEnd(Rect frame) {
            if (Absolute) return End;
            return frame.TopLeft + End * frame.Size;
        }
    }
}