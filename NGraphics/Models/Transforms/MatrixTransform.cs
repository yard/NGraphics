using System;
using System.Globalization;

namespace NGraphics.Models.Transforms
{
    public class MatrixTransform : TransformBase
    {
        public MatrixTransform(TransformBase previous = null)
            : base(previous)
        {
            Elements = new double[6];
        }

        public MatrixTransform(double[] elements, TransformBase previous = null)
            : this(previous)
        {
            if (elements == null)
                throw new ArgumentNullException("elements");
            if (elements.Length != 6)
                throw new ArgumentException("6 elements were expected");
            Array.Copy(elements, Elements, 6);
        }

        public double[] Elements;

        protected override string ToCode()
        {
            return string.Format(CultureInfo.InvariantCulture, "matrix(...)");
        }
    }
}