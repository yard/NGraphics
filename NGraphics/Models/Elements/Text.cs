using NGraphics.Custom.Codes;
using NGraphics.Custom.Interfaces;
using NGraphics.Custom.Models.Brushes;

namespace NGraphics.Custom.Models.Elements {

	/// <summary>
	/// Text.
	/// </summary>
    public class Text : Element {

		/// <summary>
		/// The frame.
		/// </summary>
        public Rect Frame;

		/// <summary>
		/// The string.
		/// </summary>
        public string String;

		/// <summary>
		/// The alignment.
		/// </summary>
        public TextAlignment Alignment;

		/// <summary>
		/// The font.
		/// </summary>
        public Font Font;

		/// <summary>
		/// Initializes a new instance of the <see cref="NGraphics.Custom.Models.Elements.Text"/> class.
		/// </summary>
		/// <param name="text">Text.</param>
		/// <param name="frame">Frame.</param>
		/// <param name="font">Font.</param>
		/// <param name="alignment">Alignment.</param>
		/// <param name="pen">Pen.</param>
		/// <param name="brush">Brush.</param>
        public Text(string text, Rect frame, Font font, TextAlignment alignment = TextAlignment.Left, Pen pen = null, BaseBrush brush = null) : base(pen, brush) {
            String = text;
            Frame = frame;
            Font = font;
            Alignment = alignment;
        }

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public override IDrawable Clone() {
			return new Text(String, Frame, Font, Alignment, Pen, Brush) {
				Id = this.Id,
				Transform = this.Transform
			};
		}

		/// <summary>
		/// Draws the element.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
        protected override void DrawElement(ICanvas canvas) {
            canvas.DrawText(String, Frame, Font, Alignment, Pen, Brush);
        }
    }
}
