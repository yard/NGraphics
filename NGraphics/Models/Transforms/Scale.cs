using System.Globalization;

namespace NGraphics.Models.Transforms
{
    public class Scale : TransformBase
    {
        public Scale(Size size, TransformBase previous = null)
            : base(previous)
        {
            Size = size;
        }

        public Scale(double dx, double dy, TransformBase previous = null)
            : this(new Size(dx, dy), previous)
        {
        }

        public Size Size;

        protected override string ToCode()
        {
            return string.Format(CultureInfo.InvariantCulture, "scale({0}, {1})", Size.Width, Size.Height);
        }
    }
}