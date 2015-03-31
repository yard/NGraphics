using System.Globalization;

namespace NGraphics.Models.Transforms
{
    public class Translate : TransformBase
    {
        public Translate(Size size, TransformBase previous = null)
            : base(previous)
        {
            Size = size;
        }

        public Translate(Point offset, TransformBase previous = null)
            : base(previous)
        {
            Size = new Size(offset.X, offset.Y);
        }

        public Translate(double dx, double dy, TransformBase previous = null)
            : this(new Size(dx, dy), previous)
        {
        }

        public Size Size;

        protected override string ToCode()
        {
            return string.Format(CultureInfo.InvariantCulture, "translate({0}, {1})", Size.Width, Size.Height);
        }
    }
}