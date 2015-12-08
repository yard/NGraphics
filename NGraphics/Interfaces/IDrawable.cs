using NGraphics.Custom.Models;

namespace NGraphics.Custom.Interfaces {

	/// <summary>
	/// I drawable.
	/// </summary>
    public interface IDrawable {
		
		/// <summary>
		/// Draw the specified canvas.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
        void Draw(ICanvas canvas);

		/// <summary>
		/// Clone this instance.
		/// </summary>
		IDrawable Clone();

		/// <summary>
		/// Tint the specified color.
		/// </summary>
		/// <param name="color">Color.</param>
		IDrawable Tint(Color color);

    }
}
