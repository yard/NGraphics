namespace NGraphics.Custom.Models.Operations {

	/// <summary>
	/// Start figure.
	/// </summary>
    public class StartFigure : PathOperation {

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public override PathOperation Clone() {
			return new StartFigure();
		}

    }
}