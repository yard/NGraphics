namespace NGraphics.Custom.Models.Operations {
	
	/// <summary>
	/// Close path.
	/// </summary>
    public class ClosePath : PathOperation {

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public override PathOperation Clone() {
			return new ClosePath();
		}

    }
}