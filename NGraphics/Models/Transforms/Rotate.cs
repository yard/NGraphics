using System.Globalization;

namespace NGraphics.Models.Transforms
{
    public class Rotate : TransformBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Rotate" /> class.
        /// </summary>
        /// <param name="angle">Angle in degrees</param>
        /// <param name="previous">Previous.</param>
        public Rotate(double angle, TransformBase previous = null)
            : base(previous)
        {
            Angle = angle;
        }

        /// <summary>
        ///     The angle in degrees.
        /// </summary>
        public double Angle;

        protected override string ToCode()
        {
            return string.Format(CultureInfo.InvariantCulture, "rotate({0})", Angle);
        }
    }
}