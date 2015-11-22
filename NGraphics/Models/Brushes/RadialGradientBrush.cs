using System.Collections.Generic;namespace NGraphics.Custom.Models.Brushes {

	/// <summary>
	/// Radial gradient brush.
	/// </summary>
    public class RadialGradientBrush : GradientBrush {

		/// <summary>
		/// The center.
		/// </summary>
        public Point Center;

		/// <summary>
		/// The focus.
		/// </summary>
        public Point Focus;

		/// <summary>
		/// The radius.
		/// </summary>
        public Size Radius;

		/// <summary>
		/// The absolute.
		/// </summary>
        public bool Absolute = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Brushes.RadialGradientBrush"/> class.
		/// </summary>
		/// <param name="stops">Stops.</param>
		protected RadialGradientBrush(List<GradientStop> stops) : base(stops) {
		}

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public override BaseBrush Clone() {
			return new RadialGradientBrush (Stops) {
				Center = this.Center,
				Focus = this.Focus,
				Radius = this.Radius,
				Absolute = this.Absolute
			};
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Brushes.RadialGradientBrush"/> class.
		/// </summary>
        public RadialGradientBrush() {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Brushes.RadialGradientBrush"/> class.
		/// </summary>
		/// <param name="relCenter">Rel center.</param>
		/// <param name="relRadius">Rel radius.</param>
		/// <param name="stops">Stops.</param>
        public RadialGradientBrush(Point relCenter, Size relRadius, params GradientStop[] stops) {
            Center = relCenter;
            Focus = relCenter;
            Radius = relRadius;
            Stops.AddRange(stops);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Brushes.RadialGradientBrush"/> class.
		/// </summary>
		/// <param name="relCenter">Rel center.</param>
		/// <param name="relRadius">Rel radius.</param>
		/// <param name="startColor">Start color.</param>
		/// <param name="endColor">End color.</param>
        public RadialGradientBrush(Point relCenter, Size relRadius, Color startColor, Color endColor) {
            Center = relCenter;
            Focus = relCenter;
            Radius = relRadius;
            Stops.Add(new GradientStop(0, startColor));
            Stops.Add(new GradientStop(1, endColor));
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Brushes.RadialGradientBrush"/> class.
		/// </summary>
		/// <param name="startColor">Start color.</param>
		/// <param name="endColor">End color.</param>
        public RadialGradientBrush(Color startColor, Color endColor) : this(new Point(0.5, 0.5), new Size(0.5), startColor, endColor) {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Brushes.RadialGradientBrush"/> class.
		/// </summary>
		/// <param name="relCenter">Rel center.</param>
		/// <param name="relRadius">Rel radius.</param>
		/// <param name="startColor">Start color.</param>
		/// <param name="midColor">Middle color.</param>
		/// <param name="endColor">End color.</param>
        public RadialGradientBrush(Point relCenter, Size relRadius, Color startColor, Color midColor, Color endColor) {
            Center = relCenter;
            Focus = relCenter;
            Radius = relRadius;
            Stops.Add(new GradientStop(0, startColor));
            Stops.Add(new GradientStop(0.5, midColor));
            Stops.Add(new GradientStop(1, endColor));
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Brushes.RadialGradientBrush"/> class.
		/// </summary>
		/// <param name="startColor">Start color.</param>
		/// <param name="midColor">Middle color.</param>
		/// <param name="endColor">End color.</param>
        public RadialGradientBrush(Color startColor, Color midColor, Color endColor) : this(new Point(0.5, 0.5), new Size(0.5), startColor, midColor, endColor) {
        }

		/// <summary>
		/// Gets the absolute center.
		/// </summary>
		/// <returns>The absolute center.</returns>
		/// <param name="frame">Frame.</param>
        public Point GetAbsoluteCenter(Rect frame) {
            if (Absolute) return Center;
            return frame.TopLeft + Center * frame.Size;
        }

		/// <summary>
		/// Gets the absolute radius.
		/// </summary>
		/// <returns>The absolute radius.</returns>
		/// <param name="frame">Frame.</param>
        public Size GetAbsoluteRadius(Rect frame) {
            if (Absolute) return Radius;
            return Radius * frame.Size;
        }

		/// <summary>
		/// Gets the absolute focus.
		/// </summary>
		/// <returns>The absolute focus.</returns>
		/// <param name="frame">Frame.</param>
        public Point GetAbsoluteFocus(Rect frame) {
            if (Absolute) return Focus;
            return frame.TopLeft + Focus * frame.Size;
        }
    }
}